import { AppBar, Toolbar, Typography } from "@material-ui/core";
import React from "react";
import PropTypes from 'prop-types'

/**
 * The basic layout of the app
 * @param {*} props 
 * @returns 
 */
function Layout(props) {
    return (
        <React.Fragment
        >
            <AppBar display="flex" elevation={0} color="primary" position="static" style={{ height: 64 }}>
                <Toolbar style={{ margin: "auto", height: 64 }}>
                    <Typography variant="h2" component="h2" color="inherit">TODO APP</Typography>
                </Toolbar>
            </AppBar>
            {props.children}
        </React.Fragment>
    );
}
Layout.propTypes = {
    children: PropTypes.node.isRequired,
}
export default Layout;