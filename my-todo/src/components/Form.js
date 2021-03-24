import React , {useState} from "react";
import { Button, TextField, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    root: {
      display: 'flex',
      flexWrap: 'wrap',
      justifyContent: "center",
    },
    textField: {
      marginLeft: theme.spacing(1),
      marginRight: theme.spacing(1),
      
      width:"50ch",
      margin: 5,
      width: "100%",
    },
    button:{
        width: "50ch",
        justifyContent: "center"
    }
  }));

function Form(props){
    const classes =useStyles();
    const [name, setName] = useState('');
    function handleChange(e){
        setName(e.target.value);
    }

    const [description, setDescription] = useState('');
    function handleDescriptionChange(e){
        setDescription(e.target.value);
    }
    return (
        <form onSubmit={handleSubmit} className={classes.root}>
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
            fullwidth
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
            fullwidth
            />
          <Button type="submit" color="primary" size="large" variant="contained" className={classes.button}>
            Add
          </Button>
        </form>
    );
      
      function handleSubmit(e) {
        e.preventDefault();
          if (name) {           
            props.addTask(name);
            setName("");
            
          }
        
      }
}

export default Form;