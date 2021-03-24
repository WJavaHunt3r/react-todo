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
    button:{
        width: "50ch",
        justifyContent: "center",
        margin: 5,
    }
  }));

function Form(props){
    const classes = useStyles();
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
                What needs to be done?
            </Typography>
            <TextField
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
            <Button type="submit" color="primary" size="large" variant="contained" onClick={handleSubmit} className={classes.button}>
              Add
            </Button>
          </Grid>
        </Paper>
    );
      
      function handleSubmit(e) {
        e.preventDefault();
          if (name) {           
            props.addTask(name, description);
            setName("");
            setDescription("");
            
          }
        
      }
}

export default Form;