
import Board from "./Board";
import { Grid } from "@material-ui/core";
import React, { useEffect, useState } from "react";

import { DragDropContext } from 'react-beautiful-dnd';

/**
 * represents the home site with boards
 * @returns 
 */
function Home() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [boards, setboards] = useState([]);

  /**
   * Handles the event of lettting a draggable drop
   * Makes the PUT API call 
   * @param {*} result = The result of dropping the item
   * @returns 
   */
  const handleOnDragEnd = (result) => {
    if (!result.destination) return;
    if (
      result.destination.droppableId === result.source.droppableId &&
      result.destination.index === result.source.index
    ) {
      return;
    }

    const sItems = boards[result.source.droppableId - 1].todoItems;
    const dItems = boards[result.destination.droppableId - 1].todoItems;
    const [todo] = sItems.splice(result.source.index, 1)
    todo.boardId = result.destination.droppableId;
    todo.priority = result.destination.index;
    dItems.splice(result.destination.index, 0, todo)

    const newSBoard = {
      ...boards[result.source.droppableId - 1],
      todoItems: sItems,
    };

    const newDBoard = {
      ...boards[result.destination.droppableId - 1],
      todoItems: dItems,
    };
    const newBoards = boards.map(b => {
      if (b.id === newDBoard.id) {
        return { ...b, todoItems: dItems }
      }
      if (b.id === newSBoard.id) { return { ...b, todoItems: sItems } }
      return b;
    })

    setboards(newBoards);

    fetch('https://localhost:5001/api/todoitems/' + result.draggableId, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(todo)
    })
      .then(
        resp => resp.json())
      .then(() =>
        getBoards()
      )
  };

  /**
   * Gets the boards from the API
   * puts them in the tasks State
   */
  function getBoards() {
    fetch("https://localhost:5001/api/boards").then(res => res.json())
      .then(
        (result) => {
          setIsLoaded(true);
          setboards(result);
        },
        (error) => {
          setIsLoaded(true);
          setError(error);
        }
      )
  }


  useEffect(() => {

    getBoards();
  }, [])
  if (error) {
    return <div>An Error occourd:{error.message}</div>
  }
  if (!isLoaded) {
    //return <div>Still loading...</div>
  }

  return (

    <React.Fragment >
      <Grid container justify="center" spacing={5} style={{ margin: 0, maxWidth: "100%" }}>
        <DragDropContext onDragEnd={handleOnDragEnd}>
          {boards.map(item => {
            return (

              <Board
                key={item.id}
                id={item.id}
                name={item.name}
                tasks={item.todoItems}
                getBoards={getBoards}
              />

            )
          }

          )}
        </DragDropContext>
      </Grid>

    </React.Fragment>

  );
}

export default Home;