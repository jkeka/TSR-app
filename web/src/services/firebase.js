import firebase from 'firebase'

/**
 * The firebase settings that were used during the original project.
 * 
 * Replace with new settings to use a different database!
 * 
 * See: https://firebase.google.com/docs/web/setup
 */

const firebaseConfig = {
  apiKey: "AIzaSyCupE0iqty7KsvZzLw5tGgZLhpe1o_5DkM",
  authDomain: "test-project1-d9370.firebaseapp.com",
  databaseURL: "https://test-project1-d9370-default-rtdb.firebaseio.com/",
  projectId: "test-project1-d9370",
  storageBucket: "test-project1-d9370.appspot.com",
  messagingSenderId: "829872808062",
  appId: "1:829872808062:web:f115b442b719f5d282f82f"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);

export default firebase