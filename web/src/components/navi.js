import React, { Component } from 'react'
import { Nav, Navbar } from 'react-bootstrap'
import { NavLink } from 'react-router-dom'
import '../css/custom.css'

export default class Navigation extends Component {
    render() {
        return (
            <Navbar bg="light" expand="lg">
            <Navbar.Brand href="/">TSR Admin page</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
              <Nav className="mr-auto">
                <NavLink className="navlink" to="/schedule">Schedule</NavLink>
                <NavLink className="navlink" to="/user">User</NavLink>
                <NavLink className="navlink" to="/location">Location</NavLink>
                <NavLink className="navlink" to="/quiz">Quiz</NavLink>
                <NavLink className="navlink" to="/reward">Reward</NavLink>
              </Nav>
            </Navbar.Collapse>
          </Navbar>
        )
    }
}