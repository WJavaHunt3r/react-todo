import { Grid, Paper} from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { useEffect, useState } from 'react';
import Todo from "./Todo";
import { Droppable ,Draggable  } from 'react-beautiful-dnd';
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
  
    return(
      
        
          <Grid item xs={3}
            className={classes.box} 
            >   
            
              <Paper                
                style={{padding:10, backgroundColor: '#aafafa',}}
                label={props.name}           
              >  
                {props.name}
                <Droppable droppableId={props.id.toString()}>
                  {(provided, snapshot) => (
                    <div
                      ref={provided.innerRef}
                      {...provided.droppableProps}
                      
                      >
                      {tasks.map((task, index) => (
            
                      <Draggable key={task.id} draggableId= {task.id.toString()} index={index}>
                        {(provided) => (
                        <div key={task.id} ref={provided.innerRef} {...provided.draggableProps} {...provided.dragHandleProps}>
                        <Todo 
                          id={task.id}
                          title = {task.title}
                          state={props.name}
                          desc={task.description}          
                          deadline = {task.deadLine}
                          priority = {task.priority}                          
                          deleteTask={deleteTask}
                          />
                        </div>
                        )}
                      </Draggable>
                      )
                    )}
                    {provided.placeholder}
                    </div>
                  )}
                </Droppable>
              </Paper>
                  
              
          </Grid>
          
        
      
    );
    
}

export default Board;