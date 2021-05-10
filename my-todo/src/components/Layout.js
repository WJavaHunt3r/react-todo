import { AppBar, Toolbar, Typography } from "@material-ui/core";
import React from "react";

function Layout(props) {
    return(
    <React.Fragment
        >
        <AppBar elevation={0} color="primary" position="static" style={{ height: 64}}>
            <Toolbar style={{ height: 64 }}>
                <Typography variant="h2" component="h2" color="inherit">TODO APP</Typography>
            </Toolbar>
        </AppBar>
        {props.children}
    </React.Fragment>
    );
}

export default Layout;