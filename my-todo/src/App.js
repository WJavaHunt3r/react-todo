
import './App.css';
import Todo from "./components/Todo";
import Form from "./components/Form";
import Layout from "./components/Layout";
import FilterButton from "./components/State";
import React, {useState} from "react";
import { nanoid } from "nanoid";
import { AppBar, Grid, List, Paper, Tabs } from '@material-ui/core';
import State from './components/State';


const FILTER_MAP = {
  Todo: task => task.state = 'Todo',
  Active: task => task.state = 'Active',
  Completed: task => task.state = 'Completed',
  Delayed: task => task.state = 'Delayed'
};
const FILTER_NAMES = Object.keys(FILTER_MAP);


function App(props) {
  const [tasks, setTasks] = useState(props.tasks);  
  
    const filterList = FILTER_NAMES.map( name => (      
        <State
          key={name} 
          name={name}
          tasks={tasks}
          setTasks={setTasks}
        ></State> 
        
    ));
 
  /*const tasksNoun = taskList.length !== 1 ? 'tasks' : 'task';
  const headingText = `${taskList.length} ${tasksNoun} remaining`;*/
  
  function addTask(name, desc) {
    const newTask = { id: "todo-" + nanoid(), name: name, desc: desc, state: 'Todo' };
    setTasks([...tasks, newTask]);
  }
  return (
   <Layout>
      <Form addTask={addTask}/>
      <Grid container justify="center" spacing={5}>
        {filterList}
      </Grid>
      <h2 id="list-heading">
        
      </h2>
      
    </Layout>
  );
  
}

export default App;
