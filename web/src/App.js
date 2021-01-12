import React from 'react';
import User from './components/user'
import Location from './components/location'
import Navigation from './components/navi'
import Home from './components/home'
import Schedule from './components/schedule'
import Reward from './components/reward'
import Quiz from './components/quiz'
import { Route, HashRouter } from 'react-router-dom'

function App() {
  return (
      <HashRouter>

        <Navigation />

        <Route exact path="/" component={Home} />
        <Route exact path="/user" component={User} />
        <Route exact path="/location" component={Location} />
        <Route exact path="/schedule" component={Schedule} />
        <Route exact path="/reward" component={Reward} />
        <Route exact path="/quiz" component={Quiz} />
        
      </HashRouter>
  );
}

export default App;
