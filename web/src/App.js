import React from 'react';
import User from './components/user'
import Location from './components/location'
import Navigation from './components/navi'
import Home from './components/home'
import Schedule from './components/schedule'
import Descriptions from './components/descriptions'
import firebase from './services/firebase'
import { BrowserRouter, Route } from 'react-router-dom'

/**
 * The base component for TSR Web UI.
 * 
 * Includes the router for the site. In the router there
 * is the Navigation bar that is included in every page. 
 * After that a component will be shown depending on the 
 * path chosen.
 * 
 * @returns BrowserRouter
 */

function App() {
  return (
      <BrowserRouter>

        <Navigation />

        <Route exact path="/" component={Home} />
        <Route exact path="/user" component={User} />
        <Route exact path="/location" component={Location} />
        <Route exact path="/schedule" 
          render={props => <Schedule {...props} firebase={firebase.database().ref()} />} />
        <Route exact path="/descriptions" component={Descriptions} />
        
      </BrowserRouter>
  );
}

export default App;
