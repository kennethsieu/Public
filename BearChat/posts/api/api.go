package api

import (
	"database/sql"
	"encoding/json"
	"errors"
	"log"
	"net/http"
	"strconv"
	"time"

	"github.com/google/uuid"
	"github.com/gorilla/mux"
)

//RegisterRoutes does stuff
func RegisterRoutes(router *mux.Router) error {
	// Why don't we put options here? Check main.go :)

	router.HandleFunc("/api/posts/{startIndex}", getFeed).Methods(http.MethodGet)
	router.HandleFunc("/api/posts/{uuid}/{startIndex}", getPosts).Methods(http.MethodGet)
	router.HandleFunc("/api/posts/create", createPost).Methods(http.MethodPost)
	router.HandleFunc("/api/posts/delete/{postID}", deletePost).Methods(http.MethodDelete)

	return nil
}

func getUUID(w http.ResponseWriter, r *http.Request) (uuid string) {
	cookie, err := r.Cookie("access_token")
	if err != nil {
		http.Error(w, errors.New("error obtaining cookie: "+err.Error()).Error(), http.StatusBadRequest)
		log.Print(err.Error())
	}
	//validate the cookie
	claims, err := ValidateToken(cookie.Value)
	if err != nil {
		http.Error(w, errors.New("error validating token: "+err.Error()).Error(), http.StatusUnauthorized)
		log.Print(err.Error())
	}
	log.Println(claims)

	return claims["UserID"].(string)
}

func getPosts(w http.ResponseWriter, r *http.Request) {

	// Load the uuid and startIndex from the url paramater into their own variables
	// Look at mux.Vars() ... -> https://godoc.org/github.com/gorilla/mux#Vars
	// make sure to use "strconv" to convert the startIndex to an integer!
	vars := mux.Vars(r)
	uuid := vars["uuid"]
	temp := vars["startIndex"]

	startIndex, _ := strconv.Atoi(temp)

	// Check if the user is authorized
	// First get the uuid from the access_token (see getUUID())
	// Compare that to the uuid we got from the url parameters, if they're not the same, return an error http.StatusUnauthorized
	// YOUR CODE HERE
	dbuuid := getUUID(w, r)

	if dbuuid != uuid {
		http.Error(w, "uiid are not the same", http.StatusUnauthorized)
		return
	}

	var posts *sql.Rows
	var err error

	/*
		-Get all that posts that matches our userID (or uuid)
		-Sort them chronologically (the database has a "postTime" field), hint: ORDER BY
		-Make sure to always get up to 25, and start with an offset of {startIndex} (look at the previous SQL homework for hints)\
		-As indicated by the "posts" variable, this query returns multiple rows
	*/
	posts, err = DB.Query("SELECT * FROM posts WHERE authorID = ? ORDER BY postTime LIMIT ?, 25", uuid, startIndex)

	// Check for errors from the query
	// YOUR CODE HERE
	if err != nil && err != sql.ErrNoRows {
		http.Error(w, errors.New("error getting post").Error(), http.StatusInternalServerError)
		log.Print(err.Error())
		return
	}

	var (
		content  string
		postID   string
		userid   string
		postTime time.Time
	)
	numPosts := 0
	// Create "postsArray", which is a slice (array) of Posts. Make sure it has size 25
	// Hint: https://tour.golang.org/moretypes/13
	postsArray := make([]Post, 25)

	for i := 0; i < 25 && posts.Next(); i++ {
		// Every time we call posts.Next() we get access to the next row returned from our query
		// Question: How many columns did we return
		// Reminder: Scan() scans the rows in order of their columns. See the variables defined up above for your convenience
		err = posts.Scan(&content, &postID, &userid, &postTime)

		// Check for errors in scanning
		if err != nil {
			http.Error(w, errors.New("error while scanning post").Error(), http.StatusInternalServerError)
			log.Print(err.Error())
			return
		}
		// Set the i-th index of postsArray to a new Post with values directly from the variables you just scanned into
		// Check post.go for the structure of a Post
		// Hint: https://gobyexample.com/structs

		//YOUR CODE HERE
		p := Post{content, postID, userid, postTime, userid}
		postsArray[i] = p
		numPosts++
	}

	posts.Close()
	err = posts.Err()
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		log.Print(err.Error())
		return
	}
	//encode fetched data as json and serve to client
	// Up until now, we've actually been counting the number of posts (numPosts)
	// We will always have *up to* 25 posts, but we can have less
	// However, we already allocated 25 spots in oru postsArray
	// Return the subarray that contains all of our values (which may be a subsection of our array or the entire array)
	json.NewEncoder(w).Encode(postsArray[:(numPosts)])
	return
}

func createPost(w http.ResponseWriter, r *http.Request) {
	// Obtain the userID from the JSON Web Token
	// See getUUID(...)
	// YOUR CODE HERE
	userUUID := getUUID(w, r)

	// Create a Post object and then Decode the JSON Body (which has the structure of a Post) into that object
	post := Post{}
	err := json.NewDecoder(r.Body).Decode(&post)
	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
	}

	// Use the uuid library to generate a post ID
	// Hint: https://godoc.org/github.com/google/uuid#New
	postID := uuid.New()

	//Load our location in PST
	pst, err := time.LoadLocation("America/Los_Angeles")
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}
	currPST := time.Now().In(pst)

	// Insert the post into the database
	// Look at /db-server/initdb.sql for a better understanding of what you need to insert
	result, err := DB.Exec("INSERT INTO posts VALUES(? , ? , ? , ?)", post.PostBody, postID, userUUID, currPST)

	// Check errors with executing the query
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}

	// Make sure at least one row was affected, otherwise return an InternalServerError
	// You did something very similar in Checkpoint 2
	value, err := result.RowsAffected()
	if value == 0 {
		if err != nil {
			http.Error(w, errors.New("Error inputing to database").Error(), http.StatusInternalServerError)
			log.Print(err.Error())
			return
		}
	}

	// What kind of HTTP header should we return since we created something?
	// Check your signup from Checkpoint 2!
	// YOUR CODE HERE
	w.WriteHeader(201)
	return
}

func deletePost(w http.ResponseWriter, r *http.Request) {
	// Get the postID to delete
	// Look at mux.Vars() ... -> https://godoc.org/github.com/gorilla/mux#Vars
	// YOUR CODE HERE
	vars := mux.Vars(r)
	postID := vars["postID"]
	// Get the uuid from the access token, see getUUID(...)
	// YOUR CODE HERE
	uuid := getUUID(w, r)

	var exists string
	//check if post exists
	err := DB.QueryRow("SELECT postID FROM posts WHERE postID = ?", postID).Scan(&exists)

	// Check for errors in executing the query
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	// Check if the post actually exists, otherwise return an http.StatusNotFound
	// YOUR CODE HERE
	if exists != postID {
		http.Error(w, err.Error(), http.StatusNotFound)
		return
	}

	// Get the authorID of the post with the specified postID
	var authorID string
	err = DB.QueryRow("SELECT authorID FROM posts WHERE postID = ?", postID).Scan(&authorID)

	// Check for errors in executing the query
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	// Check if the uuid from the access token is the same as the authorID from the query
	// If not, return http.StatusUnauthorized
	if authorID != uuid {
		http.Error(w, "Invalid User", http.StatusUnauthorized)
		return
	}

	// Delete the post since by now we're authorized to do so
	_, err = DB.Exec("DELETE FROM posts WHERE postID = ?", postID)

	// Check for errors in executing the query
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	return
}

func getFeed(w http.ResponseWriter, r *http.Request) {
	// get the start index from the url paramaters
	// based on the previous functions, you should be familiar with how to do so
	vars := mux.Vars(r)
	temp := vars["startIndex"]

	// convert startIndex to int
	startIndex, err := strconv.Atoi(temp)

	// Check for errors in converting
	// If error, return http.StatusBadRequest
	// YOUR CODE HERE
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	// Get the userID from the access_token
	// You should now be familiar with how to do so
	// YOUR CODE HERE
	uuid := getUUID(w, r)
	// Obtain all of the posts where the authorID is *NOT* the current authorID
	// Sort chronologically
	// Always limit to 25 queries
	// Always start at an offset of startIndex
	posts, err := DB.Query("SELECT * FROM posts WHERE authorID <> ? ORDER BY postTime LIMIT ?, 25", uuid, startIndex)

	// Check for errors in executing the query
	// YOUR CODE HERE
	if err != nil && err != sql.ErrNoRows {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	var (
		content  string
		postID   string
		userid   string
		postTime time.Time
	)

	// Put all the posts into an array of Max Size 25 and return all the filled spots
	// Almost exaclty like getPosts()
	numPosts := 0
	// Create "postsArray", which is a slice (array) of Posts. Make sure it has size 25
	// Hint: https://tour.golang.org/moretypes/13
	postsArray := make([]Post, 25)

	for i := 0; i < 25 && posts.Next(); i++ {

		err = posts.Scan(&content, &postID, &userid, &postTime)

		if err != nil {
			http.Error(w, errors.New("error while scanning post").Error(), http.StatusInternalServerError)
			log.Print(err.Error())
			return
		}

		//YOUR CODE HERE
		p := Post{content, postID, userid, postTime, userid}
		postsArray[i] = p
		numPosts++
	}

	err = posts.Err()

	posts.Close()
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		log.Print(err.Error())
		return
	}

	json.NewEncoder(w).Encode(postsArray[:(numPosts)])
	return
}
