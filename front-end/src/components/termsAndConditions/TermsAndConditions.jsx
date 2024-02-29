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

export default function TermsAndConditions({ open, handleClose, handleConfirm}) {
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

                await handleConfirm();

                setLoading(false);
                handleClose()

            } catch (error) {
                console.error("An error occurred:", error);
                setLoading(false);
            }
        }
    };
    const termsAndConditionsText = (
        <ul>
            <li>
                <strong>Acceptance of Terms:</strong> By accessing or using our services, you agree to be bound by these Terms and Conditions. If you disagree with any part of the terms, then you may not access the services.
            </li>
            <li>
                <strong>Content:</strong> All content provided through our services is for informational purposes only. We do not guarantee the accuracy, completeness, or usefulness of any information provided.
            </li>
            <li>
                <strong>User Conduct:</strong> You agree not to use the services for any unlawful purposes or to engage in any activity that disrupts the services or interferes with other users' ability to use the services.
            </li>
            <li>
                <strong>Privacy Policy:</strong> Our Privacy Policy outlines how we collect, use, and disclose your personal information. By using our services, you consent to the collection and use of your information as outlined in the Privacy Policy.
            </li>
            <li>
                <strong>Intellectual Property:</strong> All intellectual property rights related to the services are owned by us or our licensors. You may not use, reproduce, or distribute any content from the services without our permission.
            </li>
            <li>
                <strong>Limitation of Liability:</strong> We are not liable for any damages or losses arising from your use of the services or reliance on any information provided. In no event shall we be liable for any indirect, incidental, special, or consequential damages.
            </li>
            <li>
                <strong>Changes to Terms:</strong> We reserve the right to modify or update these Terms and Conditions at any time. Any changes will be effective immediately upon posting. It is your responsibility to review the Terms and Conditions periodically for changes.
            </li>
        </ul>
    );
    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle ><h2>Terms and Conditions</h2></DialogTitle>
            <DialogContent >
                <p>Thank you for submitting this articles, your work is a great asset to the school!</p>
                <br />
                <p>Before this article can be submitted for the Coodinator to see, there are a few Terms and Conditions:</p>
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
