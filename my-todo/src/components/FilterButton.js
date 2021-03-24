import React from "react";
import { Button, Tab } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    button:{
      margin: theme.spacing(2),
    },
  })); 
function FilterButton(props){
    const classes = useStyles();
    return(
        
        <Tab 
            //variant="contained"
            //color="default"
            //type="button" 
            //className={classes.button}
            //aria-pressed={props.isPressed}
            onClick={()=>props.setFilter(props.name)}
            label={props.name}
        >
          <span className="visually-hidden">Show </span>
          <span>{props.name}</span>
          <span className="visually-hidden"> tasks</span>
        </Tab>      
    );
}

export default FilterButton;