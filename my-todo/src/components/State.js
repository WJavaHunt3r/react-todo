import { Grid, Paper} from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import Todo from "./Todo";

const useStyles = makeStyles((theme) => ({
    button:{
      margin: theme.spacing(2),
      
    },
    box: {
      padding: 5,
      backgroundColor: '#0000ff',
    },
  })); 

  
function State(props){
    const classes = useStyles(); 
    function toggleTaskCompleted(id, state) {
      const updatedTasks = props.tasks.map(task=>{
        if (id=== task.id) {
          return {...task, state: state}
        }
        return  task;
      })
      props.setTasks(updatedTasks);
    }

  
    function deleteTask(id){
      const remainingTasks = props.tasks.filter(task=> id !== task.id);
      props.setTasks(remainingTasks);
    }
  
    function editTask(id, newName, newDesc, deadline){
      const editedTasks = props.tasks.map(task => {
        if(id === task.id){
          return{...task, name: newName, desc: newDesc, deadline: deadline}
        }
        return task;
      });
      props.setTasks(editedTasks);
    }

    const filtered = props.tasks.filter(task => {return task.state === props.name})

    function priorityDown(id){
      const curr = filtered.findIndex(task => id===task.id);
      const next = props.tasks.findIndex(task=> filtered[curr+1].id === task.id);
      const propsCurr = props.tasks.findIndex(task => id===task.id);
      const newOrder = props.tasks.map(task=>{ 
        return task});
      newOrder.splice(next,0, newOrder.splice(propsCurr,1)[0])
      props.setTasks(newOrder);
    }
    
    function priorityUp(id){
      const curr = filtered.findIndex(task => id===task.id);
      const prev = props.tasks.findIndex(task=> filtered[curr-1].id === task.id);
      const propsCurr = props.tasks.findIndex(task => id===task.id);
      const newOrder = props.tasks.map(task=>{ 
        return task});
      newOrder.splice(prev,0, newOrder.splice(propsCurr,1)[0])
      props.setTasks(newOrder);
    }

    
    const taskList = filtered.map(task => (
      <Todo 
        id={task.id}
        name = {task.name}
        state={task.state}
        desc={task.desc}
        key={task.id}
        deadline = {task.deadline}
        isFirst = {filtered.findIndex(t => task.id === t.id) === 0}
        isLast = {filtered.findIndex(t=> task.id === t.id) === filtered.length-1}
        toggleTaskCompleted={toggleTaskCompleted}
        deleteTask={deleteTask}
        editTask={editTask}
        priorityUp={priorityUp}
        priorityDown={priorityDown}
        />
      )
    );
  
    return(      
        <Grid item xs={3}
          className={classes.box} >           
          <Paper                
            style={{padding:10, backgroundColor: '#aafafa',}}
            label={props.name}           
          >  
          {props.name}
          {taskList}
          </Paper>        
        </Grid>
    );
    
}

export default State;