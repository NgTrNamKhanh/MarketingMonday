import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
} from "@mui/material";

export default function DialogDefault({ open, handleClose,contentText, headerText, title }) {
    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle ><h2>{title}</h2></DialogTitle>
            <DialogContent >
                    {headerText}
                <div style={{ maxHeight: '200px', overflowY: 'auto' }}>
                    {contentText}
                </div>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    )
}
