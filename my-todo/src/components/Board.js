
import { Box, Button, Grid, Paper } from "@material-ui/core";
import { makeStyles } from "@material-ui/core/styles";
import Todo from "./Todo";
import AddIcon from "@material-ui/icons/Add";
import { Link } from "react-router-dom";
import { Droppable, Draggable } from "react-beautiful-dnd";
import PropTypes from 'prop-types'
const useStyles = makeStyles((theme) => ({
  button: {
    margin: theme.spacing(2),
  },
  box: {
    margin: -2,
    //backgroundColor: '#000000'
  },
}));
function Board(props) {
  const classes = useStyles();
  function deleteTask(id) {
    fetch("https://localhost:5001/api/todoitems/" + id, {
      method: "DELETE",
    }).then(() => props.getBoards());
  }
  return (
    <Grid item xs={3} className={classes.box}>
      <Paper
        style={{ padding: 5, backgroundColor: "#484848", minHeight: 800 }}
        label={props.name}
      >
        <div>
          <Box display="flex">
            <Box flexGrow={1} style={{ fontWeight: 'bold', color: '#ffffff' }}>{props.name}</Box>
            <Box>
              {props.id === 1 ? (
                <Button
                  style={{ paddingRight: 4 }}
                  variant="contained"
                  color="primary"
                  justify="center"
                  component={Link}
                  to={`/new`}
                  startIcon={<AddIcon />}
                ></Button>
              ) : (
                ""
              )}
            </Box>
          </Box>
        </div>
        <hr style={{ color: "#ffffff", width: "90%" }} />
        <Droppable droppableId={props.id.toString()}>
          {(provided) => (
            <div ref={provided.innerRef} {...provided.droppableProps}>
              {props.tasks.map((task, index) => (
                <Draggable
                  key={task.id}
                  draggableId={task.id.toString()}
                  index={index}
                >
                  {(provided) => (
                    <div
                      key={task.id}
                      ref={provided.innerRef}
                      {...provided.draggableProps}
                      {...provided.dragHandleProps}
                    >
                      <Todo
                        id={task.id}
                        title={task.title}
                        state={props.name}
                        desc={task.description}
                        deadline={task.deadLine}
                        priority={task.priority}
                        deleteTask={deleteTask}
                      />
                    </div>
                  )}
                </Draggable>
              ))}
              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </Paper>
    </Grid>
  );
}
Board.propTypes = {
  getBoards: PropTypes.func.isRequired,
  name: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired,
  tasks: PropTypes.array.isRequired
}
export default Board;
