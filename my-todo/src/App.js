
import './App.css';
import NewEdit from "./components/NewEdit";
import Layout from "./components/Layout";
import React from "react";
import {
  Switch,
  Route,
} from "react-router-dom";
import Home from './components/Home';

/**
 * Initializes the layout
 * Describes the Routing
 * @returns 
 */
function App() {

  return (
    <Layout>
      <Switch>
        <Route path="/edit/:id">
          <NewEdit />
        </Route>
        <Route path="/new">
          <NewEdit />
        </Route>
        <Route path="/">
          <Home />
        </Route>
      </Switch>

    </Layout>
  );
}

export default App;
