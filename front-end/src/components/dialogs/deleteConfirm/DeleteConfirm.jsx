import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
} from "@mui/material";
import { useState } from "react";

export default function DeleteConfirm({ open, handleClose, handleConfirm }) {
    const [loading, setLoading] = useState(false);

    const confirm = async () => {
        try {
            setLoading(true);
            await handleConfirm();
            setLoading(false);
        } catch (error) {
            setLoading(false);
        }
    };
    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle ><h2>Confirm delete</h2></DialogTitle>
            <DialogContent >
                <p>You sure you want to delete this</p>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose} disabled={loading}>
                    Cancel
                </Button>
                <Button onClick={confirm} disabled={loading}>
                    {loading ? "Loading..." : "Submit"}
                </Button>
            </DialogActions>
        </Dialog>
    );
}
