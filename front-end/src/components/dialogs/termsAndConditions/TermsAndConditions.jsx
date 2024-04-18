import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    colors,
    Checkbox, 
    FormControlLabel,
    Snackbar,
    Typography
} from "@mui/material";
import { useState } from "react";

export default function TermsAndConditions({ open, handleClose, handleConfirm,termsAndConditionsText, headerText }) {
    const [loading, setLoading] = useState(false);
    const [checked, setChecked] = useState(false);
    const [showError, setShowError] = useState(false);
    const handleCheckboxChange = (event) => {
        setChecked(event.target.checked);
    };
    const confirm = async () => {
        if (!checked) {
            setShowError(true)
        }else{
            setShowError(false)
            try {
                setLoading(true);
                setChecked(false)
                await handleConfirm();
                setLoading(false);
                handleClose()

            } catch (error) {
                console.error("An error occurred:", error);
                setLoading(false);
            }
        }
    };
    
    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle ><h2>Terms and Conditions</h2></DialogTitle>
            <DialogContent >
                    {headerText}
                <div style={{ maxHeight: '200px', overflowY: 'auto' }}>
                    {termsAndConditionsText}
                </div>
                <FormControlLabel
                    control={<Checkbox checked={checked} onChange={handleCheckboxChange} color="primary" />}
                    label="I agree to the terms and conditions"
                />
            </DialogContent>
            { !checked && showError  && <Typography sx={{marginLeft: '30px'}} color="error">You must agree to the terms and conditions</Typography>}
            <DialogActions>
                <Button onClick={handleClose} disabled={loading}>
                    Cancel
                </Button>
                <Button onClick={confirm} disabled={loading}>
                    {loading ? "Loading..." : "Submit"}
                </Button>
            </DialogActions>
        </Dialog>
    )
}
