import * as React from "react";
import IconButton from "@mui/material/IconButton";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import { Link, useNavigate } from 'react-router-dom';
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import { DeleteNote } from "../Service/noteService";

const options = [
    "Edit",
    "Delete",
];

const ITEM_HEIGHT = 25;

export default function LongMenu() {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const [openDeleteDialog, setOpenDeleteDialog] = React.useState(false);
    const navigate = useNavigate(); 
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    
    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleEdit = () => {
        // Navigate to the edit page
        navigate('/edit');  // Adjust the route based on your app's routing structure
        handleClose();
    };

    const handleDelete = () => {
        // Open the confirmation dialog
        setOpenDeleteDialog(true);
        handleClose();
    };

    const handleCloseDeleteDialog = () => {
        setOpenDeleteDialog(false);
    };

    const handleConfirmDelete = () => {
        console.log("Item deletion button hit");
        DeleteNote("");
        setOpenDeleteDialog(false);
    };

    return (
        <div>
        <IconButton
            aria-label="more"
            id="long-button"
            aria-controls={open ? "long-menu" : undefined}
            aria-expanded={open ? "true" : undefined}
            aria-haspopup="true"
            onClick={handleClick}
        >
            <MoreVertIcon />
        </IconButton>
        <Menu
            id="long-menu"
            anchorEl={anchorEl}
            open={open}
            onClose={handleClose}
            slotProps={{
            paper: {
                style: {
                maxHeight: ITEM_HEIGHT * 4.5,
                width: "20ch",
                },
            },
            list: {
                "aria-labelledby": "long-button",
            },
            }}
        >
            {options.map((option) => (
            <MenuItem
                key={option}
                selected={option === "Edit"}
                onClick={option === "Edit" ? handleEdit : handleDelete}
            >
                {option}
            </MenuItem>
            ))}
        </Menu>

        <Dialog
                open={openDeleteDialog}
                onClose={handleCloseDeleteDialog}
            >
                <DialogTitle>Confirm Delete</DialogTitle>
                <DialogContent>
                    Are you sure you want to delete this item?
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseDeleteDialog} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={handleConfirmDelete} color="secondary">
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>
        </div>
    );
}
