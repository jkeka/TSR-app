import React, { Component } from 'react'
import { Nav, Navbar } from 'react-bootstrap'
import { NavLink } from 'react-router-dom'
import '../css/custom.css'
import firebase from 'firebase'

/**
 * This component includes the navigation bar that's seen
 * on the top of the site with links to different pages.
 * 
 * This component also handles logging in to Firebase.
 * The component should tell if the user is logged in, but
 * there are some issues with it. Sometimes the site tells
 * you're logged out even though you're succesfully logged in.
 */

export default class Navigation extends Component {
    constructor(props) {
      super(props)
      this.state = {status: "default", username: "", password: ""}
      this.handleSubmit = this.handleSubmit.bind(this)
      this.logOut = this.logOut.bind(this)
    }

    componentDidMount() {
      firebase.auth().onAuthStateChanged(function(user) {
        if (user) {
          console.log('User is signed in.')
        } else {
          console.log('No user is signed in.')
        }
      }); 
      const user = firebase.auth().currentUser;

      if (user) {
        console.log('User is signed in.')
        this.setState({status: "Signed in"})
      } else {
        console.log('No user is signed in.')
        this.setState({status: "Signed out"})
      }
    }
    logOut() {
      firebase.auth().signOut().then(() => {
        console.log(' Sign-out successful.')
        this.setState({status: "Signed out"})
      }).catch((error) => {
        console.log('An error happened.')
        this.setState({status: "Error signing out"})
      });
    }
    handleSubmit(e) {
      e.preventDefault()
      console.log('submit', this.state.username, this.state.password)
      firebase.auth().signInWithEmailAndPassword(this.state.username, this.state.password)
        .then((userCredential) => {
          // Signed in
          // var user = userCredential.user
          console.log('signed in')
          this.setState({status: "Signed in", username: '', password: ''})
        })
        .catch((error) => {
          console.log(error.code)
            console.log(error.message)
      })
    }
    render() {
        return (
            <Navbar bg="light" expand="lg">
            <Navbar.Brand href="/">TSR Admin page</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
              <Nav className="mr-auto">
                <NavLink className="navlink" to="/schedule">Events</NavLink>
                <NavLink className="navlink" to="/user">User</NavLink>
                <NavLink className="navlink" to="/location">Location</NavLink>
                <NavLink className="navlink" to="/descriptions">Descriptions</NavLink>
              </Nav>
            </Navbar.Collapse>

            {this.state.status !== 'Signed in' ?
            <form onSubmit={this.handleSubmit}>
              <input type="text" name="userName" autoComplete="username" placeholder="Username"
                onChange={(e) => this.setState({username: e.target.value})} />
              <input type="password" name="passWord" autoComplete="current-password" placeholder="Password"
                onChange={(e) => this.setState({password: e.target.value})} />
              <button type="submit">Login</button>
            </form>
            : '' }
            {this.state.status}
            {this.state.status === 'Signed in' ?
            <button onClick={() => this.logOut()}>Logout</button>
            : '' }
          </Navbar>
        )
    }
}