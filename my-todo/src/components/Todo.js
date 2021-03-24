import React, {useState} from "react";
import { Accordion, AccordionDetails, AccordionSummary, Button, ButtonGroup, FormGroup, Paper, TextField, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import DeleteIcon from '@material-ui/icons/Delete';
import SaveIcon from '@material-ui/icons/Save';
import EditIcon from '@material-ui/icons/Edit';
import ArrowUpwardIcon from '@material-ui/icons/ArrowUpward';
import ArrowDownwardIcon from '@material-ui/icons/ArrowDownward';
import { green } from "@material-ui/core/colors";
import DoneOutlinedIcon from '@material-ui/icons/DoneOutlined';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore'

const useStyles = makeStyles((theme) => ({
  paper:{
    marginBottom: 15,
    padding: 10,
    backgroundColor: '#fafafa',
    width:"100%"
  },
  greenButton: { 
    verticalalign:'middle',
    display: 'inline-flex',
    color: green[700],
    '&:hover':{
      color: green[50],
      backgroundColor: green[700]
    }
  },
  box: {
    height: 40,
    display: "flex",
    //padding: 8
  },
  rightBox: {
    justifyContent: "flex-end",
    alignItems: "flex-end",
    width:'auto'
  },
  heading: {
    fontSize: theme.typography.pxToRem(25),
    flexBasis: '33.33%',
    flexShrink: 0,
  },
  secondaryHeading: {
    fontSize: theme.typography.pxToRem(20),
    color: theme.palette.text.secondary,
  },
  root: {
    '& .MuiTextField-root': {
      margin: theme.spacing(1),
      width: '100%',
    },
  },
}));

export default function Todo(props) {
  const classes = useStyles();

  const [isEditing, setEditing] = useState(false);
  const [newDesc, setNewDesc] = useState('');
  function handleNewDesc(e) {
    setNewDesc(e.target.value);
  }
  const [newName, setNewName] = useState('');
  function handleNewName(e) {
    setNewName(e.target.value);
  }
  function handleSubmit(e) {
    e.preventDefault();
    props.editTask(props.id, newName, newDesc);
    setNewDesc("");
    setNewName("");
    setEditing(false);
  }

  console.log(props);
  const editingTemplate = (
    <form noValidate autoComplete="off" onSubmit={handleSubmit}>
      <FormGroup>
        <TextField 
          fullWidth
          id="name-field"          
          variant="filled"
          size="medium"
          label={`New name for: ${props.name}`}
          defaultValue={props.name}
          value={newName}
          onChange={handleNewName}
          
           />
           <TextField 
          fullWidth
          id="desc-field" 
          variant="filled"
          size="medium"
          defaultValue= "nothing"
          label={`New descieption for: ${props.name}`}
          value={newDesc}
          onChange={handleNewDesc}
          
           />
      </FormGroup>
      <ButtonGroup fullWidth variant="outlined" aria-label="outlined primary button group">
      <Button 
        color="secondary" 
        type="button"
         onClick={()=> setEditing(false)}>
          Cancel
        </Button>
        <Button 
        variant="contained"
        color="primary"
        type="submit" 
        startIcon={<SaveIcon/>}
        >
          Save
        </Button>
      </ButtonGroup>
    </form>
  );
  const viewTemplate = (

    
    <Accordion className={classes.paper}>
      <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1bh-content"
          id="panel1bh-header"
          //style={{margin:0, padding:0}}
        >
          <Typography component="h1" variant="h1" className={classes.heading}>{props.name}</Typography>
          
        </AccordionSummary>
        <AccordionDetails >
          <Typography className={classes.secondaryHeading}>
            {props.desc}
          </Typography>
        </AccordionDetails>
          
        
        <ButtonGroup  className= {`${classes.box} ${classes.rightBox}`} variant="outlined" aria-label="outlined primary button group">
          <Button  
            className={classes.greenButton} 
            onClick={() => props.toggleTaskCompleted(props.id)}
            startIcon={<DoneOutlinedIcon />}
            style={{ paddingRight: 4 }}
            />
              
          <Button
          color="primary" 
          onClick={()=>setEditing(true)}
          startIcon={<EditIcon />}
          style={{ paddingRight: 4 }}
          />
          
          <Button          
            color= "default"
            //onClick={() => props.deleteTask(props.id)}
            startIcon={<ArrowDownwardIcon />}
            style={{ paddingRight: 4 }}
          />
          <Button          
            color= "default"
           // onClick={() => props.deleteTask(props.id)}
            startIcon={<ArrowUpwardIcon />}
            style={{ paddingRight: 4 }}
          />
          <Button
                    
            variant="contained" 
            color= "secondary"
            onClick={() => props.deleteTask(props.id)}
            tyle={{ paddingRight: 4 }}
            
            startIcon={<DeleteIcon />}
          />
        </ButtonGroup>
    </Accordion>
  );

    return <li className="todo">{isEditing ? editingTemplate : viewTemplate}</li>
  }