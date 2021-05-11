import 'date-fns';
import React , {useEffect, useState} from "react";
import { Button, Grid, Paper, TextField, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { useLocation,  Link } from 'react-router-dom'
const useStyles = makeStyles((theme) => ({
    grid: {
     
      justifyContent: "center",
      padding: 10,
      marginBottom: 5,
      width:"50%"
    },
    root:{
      display: 'flex',
      flexWrap: 'wrap',
      justifyContent:"center",
      paddingBottom: 10
    },
    textField: {
      margin: 5,
      width: "100%",
    },
    dateTextField: {
      marginLeft: theme.spacing(1),
      marginRight: theme.spacing(1),
      width: 200,
    },
    button:{
        width: "50ch",
        justifyContent: "center",
        margin: 5,
    }
  }));

function NewEdit(props){
    const classes = useStyles();
    const loc = useLocation();
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [task, setTask] = useState("");
    const [newDate, setNewDate] = useState('');
    function handleNewDate(e) {
      setNewDate(e.target.value);
    }
    
    const [name, setName] = useState('');
    function handleChange(e){
        setName(e.target.value);
    }

    const [description, setDescription] = useState('');
    function handleDescriptionChange(e){
        setDescription(e.target.value);
    }
      
      useEffect(()=>{  
        if(loc.pathname !== "/new"){
          const pathname = loc.pathname.split("/")
          const id = pathname[pathname.length-1];
        
        fetch("https://localhost:5001/api/todoitems/"+id).then(res => res.json())
        .then(
          (result) => {
            setIsLoaded(true);
            setTask(result);
            setName(result.title);
            const fullDate = result.deadLine.split("T");
            setNewDate(fullDate[0]);
            setDescription(result.description);
          },         
          (error) => {
            setIsLoaded(true);
            setError(error);
          }
        )
        }
    }, [loc])
    
    if(error){
      return <div>An Error occourd:{error.message}</div>
    }
    if(!isLoaded){
      //return <div>Still loading...</div>
    }
    return (
        <Paper className={classes.root}>
          <Grid container className={classes.grid}>
            <Typography variant="h2" component="h2">
                Add a Todo!
            </Typography>
            <TextField
              required 
              id="outlined-standard"
              variant="outlined"
              className={classes.textField}
              autoComplete="off"
              value={name}
              onChange={handleChange}
              label="Todo title"
              fullWidth
            />
            <TextField
              required 
              id="outlined-multiline-static"
              className={classes.textField}
              multiline
              rows={4}
              value={description}
              label="Description"
              variant="outlined"
              onChange={handleDescriptionChange}
              fullWidth
              />
              
              <TextField
                required
                id="date"
                label="Deadline"
                type="date"
                //defaultValue="2021-03-25"
                className={classes.dateTextField}
                value={newDate}
                onChange={handleNewDate}
                InputLabelProps={{
                  shrink: true,
                }}
            />
                
                  <Button type="submit" color="primary" size="large" variant="contained" onClick={handleSubmit}  className={classes.button}>
                    Add
                  </Button>
                  <Button id="back" type="submit" color="primary" size="large" variant="contained"  component={Link} to={`/`}   className={classes.button}>
                    Home
                  </Button>
                
          </Grid>
        </Paper>
    );
      
      function handleSubmit(e) {
        e.preventDefault();
          if (name && description && newDate) {
            const todoItem = {
              "id":0,
              "title": name,
              "description": description,
              "deadLine": newDate,
              "priority":0,
              "boardId":0
            };

            (loc.pathname !== "/new") ? editTask(todoItem) : addTask(todoItem);

            
            setName("");
            setDescription("");
            setNewDate("");
          }       
      }
      function addTask(todoItem) {
        fetch('https://localhost:5001/api/todoitems', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify( todoItem )
      })
        .then(data => data.json())
        .then(
          (result) => {
            //console.log(result);
            
          })
          
      }

      function  editTask(todoItem) {
        todoItem.id=task.id;
        todoItem.boardId=task.boardId;
        fetch('https://localhost:5001/api/todoitems/' + task.id, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(todoItem )
      })
        .then(data => data.json())
        .then(
          (result) => {
            //console.log(result);
          })
      
      }
}

export default NewEdit;