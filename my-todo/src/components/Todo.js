import React from "react";
import 'date-fns';
import { Accordion, AccordionDetails, AccordionSummary, Button, ButtonGroup, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import { green } from "@material-ui/core/colors";
import ExpandMoreIcon from '@material-ui/icons/ExpandMore'
import { Link} from "react-router-dom";
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
  greenButtonOff: {     
    color: green[50],
    backgroundColor: green[700],
    '&:hover':{
      verticalalign:'middle',
    display: 'inline-flex',
    color: green[700],
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
  dateTextField: {
    marginLeft: theme.spacing(1),
    marginRight: theme.spacing(1),
    width: 200,
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

  return (

    
    <Accordion className={classes.paper} >
      <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1bh-content"
          id="panel1bh-header"
          style={{margin:0, padding:0}}
        >
          
          <Typography component="h1" variant="h1" className={classes.heading}>{props.title}</Typography>
          <Typography className={classes.secondaryHeading}>{props.deadline.split("T")[0]}</Typography>
        </AccordionSummary>
        <AccordionDetails >
          <Typography className={classes.secondaryHeading}>
            {props.desc}
          </Typography>
        </AccordionDetails>
          
        
        <ButtonGroup  className= {`${classes.box} ${classes.rightBox}`} variant="outlined" aria-label="outlined primary button group">
          
            <Button
            color="primary" 
            component={Link}
            to={`/edit/${props.id}`}            
            startIcon={<EditIcon />}
            style={{ paddingRight: 4 }}

            />
          
          <Button
                    
            //variant="contained" 
            color= "secondary"
            onClick={() => props.deleteTask(props.id)}
            style={{ paddingRight: 4 }}
            
            startIcon={<DeleteIcon />}
          />
        </ButtonGroup>
    </Accordion>
  );

  }