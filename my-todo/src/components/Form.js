import 'date-fns';
import React , {useState} from "react";
import { Button, Grid, Paper, TextField, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

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

function Form(props){
    const classes = useStyles();
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
                  <Button type="submit" color="primary" size="large" variant="contained" onClick={handleSubmit} className={classes.button}>
              Add
            </Button>
          </Grid>
        </Paper>
    );
      
      function handleSubmit(e) {
        e.preventDefault();
          if (name && description) {           
            props.addTask(name, description, newDate);
            setName("");
            setDescription("");
            setNewDate("");
          }
        
      }
}

export default Form;