import 'date-fns';
import React, { useEffect, useState } from "react";
import { Button, Grid, Modal, Paper, TextField, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { useLocation, Link } from 'react-router-dom'
//import ModalWindow from './ModalWindow';

function rand() {
  return Math.round(Math.random() * 20) - 10;
}

function getModalStyle() {
  const top = 50 + rand();
  const left = 50 + rand();

  return {
    top: `${top}%`,
    left: `${left}%`,
    transform: `translate(-${top}%, -${left}%)`,
  };
}



const useStyles = makeStyles((theme) => ({
  grid: {

    justifyContent: "center",
    padding: 10,
    marginBottom: 5,
    width: "50%"
  },
  root: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: "center",
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
  button: {
    width: "50ch",
    justifyContent: "center",
    margin: 5,
  },
  paper: {
    position: 'absolute',
    width: 400,
    backgroundColor: theme.palette.background.paper,
    border: '2px solid #000',
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
}));

/**
 * The site for adding or editing a Todo
 * determines by the url
 * @returns 
 */
function NewEdit() {
  const classes = useStyles();
  const [modalStyle] = useState(getModalStyle);
  const loc = useLocation();
  const [modalOpen, setModalOpen] = useState(false);
  const [modalTitle, setModalTitle] = useState("");
  const [modalTDesc, setModalDesc] = useState("");
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [task, setTask] = useState("");
  const handleClose = () => {
    setModalOpen(false);
  };
  const [newDate, setNewDate] = useState('');
  /**
   * sets the new date when its changed
   * @param {*} e 
   */
  function handleNewDate(e) {
    setNewDate(e.target.value);
  }

  const [name, setName] = useState('');

  /**
   * sets the new name when its changed
   * @param {*} e 
   */
  function handleChange(e) {
    setName(e.target.value);
  }

  const [description, setDescription] = useState('');
  /**
   * Sets the new description when its changed
   * @param {*} e 
   */
  function handleDescriptionChange(e) {
    setDescription(e.target.value);
  }
  const [isNew, setIsNew] = useState(true);

  /**
   * gets the boards from the api upon loading
   */
  useEffect(() => {
    if (loc.pathname !== "/new") {
      setIsNew(false);
      const pathname = loc.pathname.split("/")
      const id = pathname[pathname.length - 1];

      fetch("https://localhost:5001/api/todoitems/" + id).then(res => res.json())
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

  if (error) {
    return <div style={{ color: "white" }}>An Error occourd:{error.message}</div>
  }
  if (!isLoaded) {
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

        <Button type="submit" color="primary" size="large" variant="contained" onClick={handleSubmit} className={classes.button}>
          {isNew ? 'Add' : 'edit'}
        </Button>
        <Button id="back" type="submit" color="primary" size="large" variant="contained" component={Link} to={`/`} className={classes.button}>
          Home
        </Button>

        <Modal
          open={modalOpen}
          onClose={handleClose}
          aria-labelledby="simple-modal-title"
          aria-describedby="simple-modal-description"
        >
          <div style={{ modalStyle }} className={classes.paper}>
            <h2 id="simple-modal-title">{modalTitle}</h2>
            <p id="simple-modal-description">
              {modalTDesc}
            </p>
            <button color="primary" size="large" variant="contained" type="button" onClick={handleClose}>
              Got it
            </button>
          </div>


        </Modal>
      </Grid>
    </Paper>
  );
  /**
   * Gets called when the add button was clicked
   * adds or edits the todo dependig on the situation 
   * @param {*} e 
   */
  function handleSubmit(e) {
    e.preventDefault();
    if (name && description && newDate) {
      const todoItem = {
        "id": 0,
        "title": name,
        "description": description,
        "deadLine": newDate,
        "priority": task.priority,
        "boardId": 0
      };

      (loc.pathname !== "/new") ? editTask(todoItem) : addTask(todoItem);
    }
  }

  /**
   * Handles the api call of adding a new todo
   * @param {Todo} todoItem = the todoItem to be added
   */
  function addTask(todoItem) {
    fetch('https://localhost:5001/api/todoitems', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(todoItem)
    })
      .then(data => data.json())
      .then(
        () => {
          setModalTitle("Successfully added new Todo");
          setModalOpen(true);
          setName("");
          setDescription("");
          setNewDate("");
        }), (error) => {
          setIsLoaded(true);
          setError(error);
          setModalTitle("An error occourd while adding the Todo");
          setModalDesc(error);
          setModalOpen(true);
        }

  }

  /**
   * Handles the api call of editing(PUT) a todo
   * @param {Todo} todoItem = The todoItem to be deited
   */
  function editTask(todoItem) {
    todoItem.id = task.id;
    todoItem.boardId = task.boardId;
    fetch('https://localhost:5001/api/todoitems/' + task.id, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(todoItem)
    })
      .then(data => data.json())
      .then(
        () => {
          setModalTitle(" Todo Successfully edited");
          setModalOpen(true);

        }), (error) => {
          setIsLoaded(true);
          setError(error);
          setModalTitle("An error occourd while editing the Todo");
          setModalDesc(error);
          setModalOpen(true);
        }

  }
}

export default NewEdit;