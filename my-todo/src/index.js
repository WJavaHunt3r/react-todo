import React from 'react';
import ReactDOM from 'react-dom';
//import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
//import CssBaseline from '@material-ui/core/CssBaseline';
//import { ThemeProvider } from '@material-ui/core/styles';


const DATA = [
  { id: "todo-0", name: "Eat", desc: "dasd", state: 'Todo' },
  { id: "todo-1", name: "Sleep", desc: "dasd", state: 'Completed' },
  { id: "todo-2", name: "Repeat", desc: "das", state: 'Delayed' }
];
ReactDOM.render(
  //<CssBaseline />
  <App tasks = {DATA} />,
  document.getElementById('root')
);


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
