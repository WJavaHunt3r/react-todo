import React, { useState } from "react";
import { Grid, List, Paper, Tab } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import Todo from "./Todo";

const useStyles = makeStyles((theme) => ({
    button:{
      margin: theme.spacing(2),
      
    },
    box: {
      padding: 5,
      backgroundColor: '#0000ff',
    },
  })); 

  
function State(props){
    const classes = useStyles(); 
    function toggleTaskCompleted(id) {
      const updatedTasks = props.tasks.map(task=>{
        if (id=== task.id) {
          return {...task, state: 'Completed'}
        }
        return  task;
      })
      props.setTasks(updatedTasks);
    }
  
    function deleteTask(id){
      const remainingTasks = props.tasks.filter(task=> id !== task.id);
      props.setTasks(remainingTasks);
    }
  
    function editTask(id, newName, newDesc){
      const editedTasks = props.tasks.map(task => {
        if(id === task.id){
          return{...task, name: newName, desc: newDesc}
        }
        return task;
      });
      props.setTasks(editedTasks);
    }

    const filtered = props.tasks.filter(task => {return task.state === props.name})
    console.log(props);
    const taskList = filtered.map(task => (
      <Todo 
        id={task.id}
        name = {task.name}
        state={task.state}
        desc={task.desc}
        key={task.id}
        toggleTaskCompleted={toggleTaskCompleted}
        deleteTask={deleteTask}
        editTask={editTask}
        />
      )
    );
  
    return(      
        <Grid item xs={3}
          className={classes.box} >           
          <Paper    
            
            style={{padding:10, backgroundColor: '#aafafa',}}
            label={props.name}
            
          >  
          {props.name}
          {taskList}
          </Paper>
          
        </Grid>
    );
    
}

export default State;