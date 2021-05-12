import React from "react";
import 'date-fns';
import { Accordion, AccordionDetails, AccordionSummary, Button, ButtonGroup, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore'
import { Link } from "react-router-dom";
import PropTypes from 'prop-types'
const useStyles = makeStyles((theme) => ({
  paper: {
    marginBottom: 15,
    padding: 10,
    backgroundColor: '#d0d0d0',
    width: "100%"
  },
  box: {
    height: 40,
    display: "flex",
    //padding: 8
  },
  rightBox: {
    justifyContent: "flex-end",
    alignItems: "flex-end",
    width: 'auto'
  },
  heading: {
    fontSize: theme.typography.pxToRem(25),
    flexBasis: '66.66%',
    flexShrink: 0,
    color: "#000000",
    fontWeight: 'bold'
  },
  secondaryHeading: {
    fontSize: theme.typography.pxToRem(20),
    color: "#0f0f0f",
  },
  root: {
    '& .MuiTextField-root': {
      margin: theme.spacing(1),
      width: '100%',
    },
  },
}));

function Todo(props) {
  const classes = useStyles();

  return (


    <Accordion className={classes.paper} >
      <AccordionSummary
        display="flex"
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel1bh-content"
        id="panel1bh-header"
        style={{ margin: 0, padding: 0 }}
      >

        <Typography flexGrow={1} component="h1" variant="h1" className={classes.heading}>{props.title}</Typography>
        <Typography className={classes.secondaryHeading}>{props.deadline.split("T")[0]}</Typography>

      </AccordionSummary>
      <AccordionDetails >
        <Typography className={classes.secondaryHeading}>
          {props.desc}
        </Typography>
      </AccordionDetails>
      <ButtonGroup className={`${classes.box} ${classes.rightBox}`} variant="outlined" aria-label="outlined primary button group">

        <Button
          color="primary"
          component={Link}
          to={`/edit/${props.id}`}
          startIcon={<EditIcon />}
          style={{ paddingRight: 4 }}

        />

        <Button

          //variant="contained" 
          color="secondary"
          onClick={() => props.deleteTask(props.id)}
          style={{ paddingRight: 4 }}

          startIcon={<DeleteIcon />}
        />
      </ButtonGroup>
    </Accordion>
  );

}
Todo.propTypes = {
  deleteTask: PropTypes.func.isRequired,
  title: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired,
  deadline: PropTypes.string.isRequired,
  desc: PropTypes.string.isRequired
}
export default Todo;