
import Board from "./Board";
import { Button, Grid } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import AddIcon from '@material-ui/icons/Add';
import { Link} from "react-router-dom";

const FILTER_MAP = {
    Todo: task => task.state = 'Todo',
    Active: task => task.state = 'Active',
    Completed: task => task.state = 'Completed',
    Delayed: task => task.state = 'Delayed'
  };
  const FILTER_NAMES = Object.keys(FILTER_MAP);

function Home(props){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [boards, setboards] = useState([]); 
    /*function addTask(name, desc, deadline) {
        const newTask = { id: "todo-" + nanoid(), name: name, desc: desc, state: 'Todo', deadline: deadline};
        setTasks([...tasks, newTask]);
      }*/
    const filterList = FILTER_NAMES.map( name => (      
        <Board
          key={name} 
          name={name}
          tasks={props.tasks}
          //setTasks={setTasks}
        ></Board> 
    ));

    useEffect(()=>{
        fetch("https://localhost:5001/api/boards").then(res => res.json())
        .then(
          (result) => {
            setIsLoaded(true);
            setboards(result);
            console.log(result);
          },         
          (error) => {
            setIsLoaded(true);
            setError(error);
          }
        )
    }, [])

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
            <Grid item xs={12}>
                <Grid container justify="center" spacing={4} >
                
                {filterList}
                </Grid>
            </Grid>
        </Grid>
        
       </React.Fragment>
    );
}

export default Home;