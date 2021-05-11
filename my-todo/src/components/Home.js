
import Board from "./Board";
import { Button, Grid } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import AddIcon from '@material-ui/icons/Add';
import { Link} from "react-router-dom";
import { DragDropContext  } from 'react-beautiful-dnd';

function Home(props){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [boards, setboards] = useState([]); 
   
    function handleOnDragEnd(result) {
        if(!result.destination) return;
        const todo = boards[result.source.droppableId-1].todoItems[result.source.index];
        todo.boardId = result.destination.droppableId;
        todo.priority = result.destination.index;
        fetch('https://localhost:5001/api/todoitems/' + result.draggableId, {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(todo )
          })
            .then(data => data.json())
            .then(
              (result) => {
                getBoards();
              })          
    }

    function getBoards(){
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
        )}
    

    useEffect(()=>{
        getBoards();
    }, [])
    if(error){
        return <div>An Error occourd:{error.message}</div>
      }
      if(!isLoaded){
        //return <div>Still loading...</div>
      }

    return(
        
        <React.Fragment>
        <Grid container>
            <Grid item xs={12}>
            <Link to="/new">
                <Button variant="contained" color="primary" justify="center" startIcon={<AddIcon/>}>                   
                        Add Todo!                   
                </Button>
            </Link>
            </Grid>
                <Grid container justify="center" spacing={4}>
                    <DragDropContext onDragEnd={handleOnDragEnd}>
                        {boards.map( item => {
                            return(  

                                <Board
                                    key={item.id}
                                    id={item.id}
                                    name={item.name}
                                    //tasks={props.tasks}
                                />
                            
                            )}
                            
                        )}
                    </DragDropContext>
                </Grid>
            </Grid>
       
       </React.Fragment>
       
    );
}

export default Home;