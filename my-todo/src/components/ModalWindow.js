import React, { useState } from "react";
import Modal from '@material-ui/core/Modal';
import { makeStyles } from '@material-ui/core/styles';
import PropTypes from 'prop-types'

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
    paper: {
        position: 'absolute',
        width: 400,
        backgroundColor: theme.palette.background.paper,
        border: '2px solid #000',
        boxShadow: theme.shadows[5],
        padding: theme.spacing(2, 4, 3),
    },
}));

function ModalWindow(props) {
    const classes = useStyles();
    const [modalStyle] = useState(getModalStyle);
    const [open, setOpen] = useState(true);
    const handleClose = () => {
        setOpen(false);
    };

    return (
        <Modal
            open={open}
            onClose={handleClose}
            aria-labelledby="simple-modal-title"
            aria-describedby="simple-modal-description"
        >
            <div style={{ modalStyle }} className={classes.paper}>
                <h2 id="simple-modal-title">{props.title}</h2>
                <p id="simple-modal-description">
                    {props.Description}
                </p>
                <button type="button" onClick={handleClose}>
                    Got it
            </button>
            </div>


        </Modal>
    )
}
ModalWindow.propTypes = {
    title: PropTypes.string.isRequired,
    Description: PropTypes.string.isRequired,
    open: PropTypes.bool.isRequired
}
export default ModalWindow;