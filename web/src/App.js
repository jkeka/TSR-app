import React from 'react';
import User from './components/user'
import Location from './components/location'
import Navigation from './components/navi'
import Home from './components/home'
import Schedule from './components/schedule'
import Event from './components/schedule/event'
import Descriptions from './components/descriptions'
import firebase from './services/firebase'


// import { Route, IndexRoute, HashRouter } from 'react-router-dom'
import { BrowserRouter, Route } from 'react-router-dom'

function App() {
  return (
      <BrowserRouter>

        <Navigation />

        <Route exact path="/" component={Home} />
        <Route exact path="/user" component={User} />
        <Route exact path="/location" component={Location} />
        <Route exact path="/event" component={Event} />
        <Route exact path="/schedule" render={props => <Schedule {...props} firebase={firebase.database().ref()} />} />
        <Route exact path="/descriptions" component={Descriptions} />
        
      </BrowserRouter>
  );
}

export default App;
