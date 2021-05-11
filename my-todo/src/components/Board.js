import { Grid, Paper} from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { useEffect, useState } from 'react';
import Todo from "./Todo";
import { DragDropContext,Droppable ,Draggable  } from 'react-beautiful-dnd';
//import {useHistory} from "react-router-dom"
const useStyles = makeStyles((theme) => ({
    button:{
      margin: theme.spacing(2),
      
    },
    box: {
      padding: 5,
      backgroundColor: '#0000ff',
    },
  })); 

  
function Board(props){
    const classes = useStyles(); 
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [tasks, setTasks] = useState([]);
    useEffect(()=>{
      //console.log(props);
      getTasks(props);
  }, [props])
  if(error){
    return <div>An Error occourd:{error.message}</div>
  }
  if(!isLoaded){
    //return <div>Still loading...</div>
  }
    function toggleTaskCompleted(id, state) {
      const updatedTasks = tasks.map(task=>{
        if (id=== task.id) {
          return {...task, state: state}
        }
        return  task;
      })
      setTasks(updatedTasks);
    }

  
    function deleteTask(id){
      fetch("https://localhost:5001/api/todoitems/"+id, { method: 'DELETE' })
        .then(() => getTasks(props));
      
    }

    function getTasks(props){
      fetch("https://localhost:5001/api/boards/"+props.id).then(res => res.json())
      .then(
        (result) => {
          setIsLoaded(true);
          setTasks(result.todoItems);      

        },         
        (error) => {
          setIsLoaded(true);
          setError(error);
        }
      )
    }

    

    function priorityDown(id){
      const curr = tasks.findIndex(task => id===task.id);
      const next = tasks.findIndex(task=> tasks[curr+1].id === task.id);
      const propsCurr = tasks.findIndex(task => id===task.id);
      const newOrder = tasks.map(task=>{ 
        return task});
      newOrder.splice(next,0, newOrder.splice(propsCurr,1)[0])
      props.setTasks(newOrder);
    }
    
    function priorityUp(id){
      const curr = tasks.findIndex(task => id===task.id);
      const prev = tasks.findIndex(task=> tasks[curr-1].id === task.id);
      const propsCurr = tasks.findIndex(task => id===task.id);
      const newOrder = tasks.map(task=>{ 
        return task});
      newOrder.splice(prev,0, newOrder.splice(propsCurr,1)[0])
      props.setTasks(newOrder);
    }

    
    const taskList = tasks.map((task, index) => (
      
      <Draggable key={task.id} draggableId= {task.id.toString()} index={index}>
        {(provided) => (
        <li ref={provided.innerRef} {...provided.draggableProps} {...provided.dragHandleProps}>
        <Todo 
          id={task.id}
          title = {task.title}
          state={props.name}
          desc={task.description}          
          deadline = {task.deadLine}
          priority = {task.priority}
          isFirst = {tasks.findIndex(t => task.id === t.id) === 0}
          isLast = {tasks.findIndex(t=> task.id === t.id) === tasks.length-1}
          toggleTaskCompleted={toggleTaskCompleted}
          deleteTask={deleteTask}
          priorityUp={priorityUp}
          priorityDown={priorityDown}
          />
        </li>
        )}
      </Draggable>
      )
    );
  
    return(
      
        
          <Grid item xs={3}
            className={classes.box} 
            >   
            
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

export default Board;