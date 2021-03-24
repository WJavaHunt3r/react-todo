import React, {useState} from "react";
import { Button, ButtonGroup, Checkbox, FormControl, FormControlLabel, FormGroup, TextField } from '@material-ui/core';
import { makeStyles, withTheme } from '@material-ui/core/styles';
import DeleteIcon from '@material-ui/icons/Delete';
import SaveIcon from '@material-ui/icons/Save';
import EditIcon from '@material-ui/icons/Edit';
import ArrowUpwardIcon from '@material-ui/icons/ArrowUpward';
import ArrowDownwardIcon from '@material-ui/icons/ArrowDownward';
import { green } from "@material-ui/core/colors";
import DoneOutlinedIcon from '@material-ui/icons/DoneOutlined';

const useStyles = makeStyles((theme) => ({
  button:{
    margin: theme.spacing(2),
    width: 15,
  },
  greenButton: { 
    verticalAlign:'middle',
    display: 'inline-flex',
    color: green[700],
    '&:hover':{
      color: green[50],
      backgroundColor: green[700]
    }
  },
  box: {
    height: 50,
    display: "flex",
    padding: 8
  },
  rightBox: {
    justifyContent: "flex-end",
    alignItems: "flex-end"
  },
  root: {
    '& .MuiTextField-root': {
      margin: theme.spacing(1),
      width: '25ch',
    },
  },
}));

export default function Todo(props) {
  const classes = useStyles();
  const [isEditing, setEditing] = useState(false);
  const [newName, setNewName] = useState('');
  function handleChange(e) {
    setNewName(e.target.value);
  }
  function handleSubmit(e) {
    e.preventDefault();
    props.editTask(props.id, newName);
    setNewName("");
    setEditing(false);
  }

  const editingTemplate = (
    <form className={classes.root} noValidate autoComplete="off" onSubmit={handleSubmit}>
      <FormGroup>
        <TextField 
          fullWidth
          id="outlined-basic" 
          margin= "none"
          variant="outlined"
          size="medium"
          label={`New name for: ${props.name}`}
          value={newName}
          onChange={handleChange}
           />
      </FormGroup>
      <ButtonGroup  className= {`${classes.box} ${classes.rightBox}`} variant="outlined" aria-label="outlined primary button group">
          <Button  
            className={classes.greenButton} 
            onClick={() => props.toggleTaskCompleted(props.id)}
            startIcon={<DoneOutlinedIcon />}
            style={{ paddingRight: 4 }}/>
              
          <Button
          color="primary" 
          verticalAlign="center"
          onClick={()=>setEditing(true)}
          startIcon={<EditIcon />}
          style={{ paddingRight: 4 }}
          />
          
          <Button          
            color= "default"
            onClick={() => props.deleteTask(props.id)}
            startIcon={<ArrowDownwardIcon />}
            style={{ paddingRight: 4 }}
          />
          <Button          
            color= "default"
            onClick={() => props.deleteTask(props.id)}
            startIcon={<ArrowUpwardIcon />}
            style={{ paddingRight: 4 }}
          />
          <Button
                    
            variant="contained" 
            color= "secondary"
            onClick={() => props.deleteTask(props.id)}
            style={{ paddingRight: 4 }}
            
            startIcon={<DeleteIcon />}
          />
        </ButtonGroup>
    </form>
  );
  const viewTemplate = (
    <FormGroup>
      {props.name}
          
        
        <ButtonGroup  className= {`${classes.box} ${classes.rightBox}`} variant="outlined" aria-label="outlined primary button group">
          <Button  
            className={classes.greenButton} 
            onClick={() => props.toggleTaskCompleted(props.id)}
            startIcon={<DoneOutlinedIcon />}
            style={{ paddingRight: 4 }}/>
              
          <Button
          color="primary" 
          verticalAlign="center"
          onClick={()=>setEditing(true)}
          startIcon={<EditIcon />}
          style={{ paddingRight: 4 }}
          />
          
          <Button          
            color= "default"
            onClick={() => props.deleteTask(props.id)}
            startIcon={<ArrowDownwardIcon />}
            style={{ paddingRight: 4 }}
          />
          <Button          
            color= "default"
            onClick={() => props.deleteTask(props.id)}
            startIcon={<ArrowUpwardIcon />}
            style={{ paddingRight: 4 }}
          />
          <Button
                    
            variant="contained" 
            color= "secondary"
            onClick={() => props.deleteTask(props.id)}
            style={{ paddingRight: 4 }}
            
            startIcon={<DeleteIcon />}
          />
        </ButtonGroup>
    </FormGroup>
  );

    return <li className="todo">{isEditing ? editingTemplate : viewTemplate}</li>
  }