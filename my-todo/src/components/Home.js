
import Board from "./Board";
import { Button, Grid } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import AddIcon from '@material-ui/icons/Add';
import { Link} from "react-router-dom";
import { DragDropContext,Droppable ,Draggable  } from 'react-beautiful-dnd';

function Home(props){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [boards, setboards] = useState([]); 
    /*function addTask(name, desc, deadline) {
        const newTask = { id: "todo-" + nanoid(), name: name, desc: desc, state: 'Todo', deadline: deadline};
        setTasks([...tasks, newTask]);
      }*/
    const filterList = boards.map( item => {
        return(  
        
            <Droppable key={item.id} droppableId={item.id.toString()}>
                {(provided) => ( 
                <Grid container justify="center" spacing={4}  className={item.id.toString()}{...provided.droppableProps} ref={provided.innerRef}>
                    <Board
                        
                        id={item.id}
                        name={item.name}
                        tasks={props.tasks}
                        //setTasks={setTasks}
                    ></Board>
                    {provided.placeholder}
                </Grid>
                )}
            </Droppable>
        
        )}
        
    );

    useEffect(()=>{
        fetch("https://localhost:5001/api/boards").then(res => res.json())
        .then(
          (result) => {
            setIsLoaded(true);
            setboards(result);
            //console.log(result);
          },         
          (error) => {
            setIsLoaded(true);
            setError(error);
          }
        )
    }, [])
    if(error){
        return <div>An Error occourd:{error.message}</div>
      }
      if(!isLoaded){
        //return <div>Still loading...</div>
      }

    return(
        
        <React.Fragment>
        <Grid container spacing={5}>
            <Grid item xs={12}>
            <Link to="/new">
                <Button variant="contained" color="primary" justify="center" startIcon={<AddIcon/>}>                   
                        Add Todo!                   
                </Button>
            </Link>
            </Grid>
            
            <Grid item xs={12} >
            
            <Grid container justify="center" spacing={4}>
            <DragDropContext>
            {boards.map( item => {
                return(  
                
                    <Droppable key={item.id} droppableId={item.id.toString()}>
                        {(provided) => ( 
                        <div className={item.id.toString()}{...provided.droppableProps} ref={provided.innerRef}>
                            <Board
                                
                                id={item.id}
                                name={item.name}
                                tasks={props.tasks}
                                //setTasks={setTasks}
                            ></Board>
                            {provided.placeholder}
                        </div>
                        )}
                    </Droppable>
                
                )}
                
            )}
            </DragDropContext>
            </Grid>
            
            </Grid>
            
            
            
        </Grid>
        
       </React.Fragment>
       
    );
}

export default Home;