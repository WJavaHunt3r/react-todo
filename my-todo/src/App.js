
import './App.css';
import NewEdit from "./components/NewEdit";
import Layout from "./components/Layout";
import React from "react";
import {
  Switch,
  Route,
} from "react-router-dom";
import Home from './components/Home';





function App(props) {
   
  
    
    
 
  /*const tasksNoun = taskList.length !== 1 ? 'tasks' : 'task';
  const headingText = `${taskList.length} ${tasksNoun} remaining`;*/
  
  
  return (
   <Layout>
     <Switch>
       <Route path="/edit/:id">
          <NewEdit/>
       </Route>
       <Route path="/new">
         <NewEdit/>
       </Route>
       <Route path="/">
         <Home tasks={props.tasks}/>
       </Route>
     </Switch>
     {/* <NewEdit addTask={addTask}/>
      {console.log(tasks)}
      <Grid container justify="center" spacing={5}>
        {filterList}
      </Grid>
      <Typography id="list-heading">
        
  </Typography>*/}
      
    </Layout>
  );
  
}

export default App;
