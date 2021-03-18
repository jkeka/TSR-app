import React from 'react';
import User from './components/user'
import Location from './components/location'
import Navigation from './components/navi'
import Home from './components/home'
// import Schedule from './components/schedule'
import Event from './components/schedule/event'
import Reward from './components/reward'
import Quiz from './components/quiz'
import Descriptions from './components/descriptions'
import Settings from './components/settings/theSettings'


// import { Route, IndexRoute, HashRouter } from 'react-router-dom'
import { BrowserRouter, Route } from 'react-router-dom'

function App() {
  return (
      <BrowserRouter>

        <Navigation />

        <Route exact path="/" component={Home} />
        <Route exact path="/user" component={User} />
        <Route exact path="/location" component={Location} />
        <Route exact path="/schedule" component={Event} />
        <Route exact path="/reward" component={Reward} />
        <Route exact path="/quiz" component={Quiz} />
        <Route exact path="/descriptions" component={Descriptions} />
        <Route exact path="/settings" component={Settings} />
        
      </BrowserRouter>
  );
}

export default App;
