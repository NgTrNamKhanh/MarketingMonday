import { Button, Checkbox, Dialog, DialogActions, DialogContent, DialogTitle, FormControlLabel, Typography } from '@mui/material'
import React, { useState } from 'react'

export default function Confirm({ open, handleClose, handleConfirm}) {
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
    
const teacherSubmissionTerms = (
  <div>
    <p>
      <strong>By submitting an article:</strong> You, as the teacher, agree to the following terms and conditions:
    </p>
    <ul>
      <li>
        <strong>Verification:</strong> You acknowledge that you are responsible for verifying the authenticity and originality of the submitted article.
      </li>
      <li>
        <strong>Public Disclosure:</strong> You agree that upon successful verification, the submitted article may be made public for educational purposes.
      </li>
      <li>
        <strong>Confidentiality:</strong> You agree to handle all submissions with confidentiality and shall not disclose any personally identifiable information of the student without their consent, except as required by law or educational policy.
      </li>
      <li>
        <strong>Ownership:</strong> You acknowledge that the student retains ownership and copyright of their work, and you shall not claim ownership or reproduce the work for commercial purposes without the student's explicit consent.
      </li>
      <li>
        <strong>Agreement:</strong> By submitting the article, you confirm that you have read, understood, and agree to abide by these terms and conditions.
      </li>
    </ul>
  </div>
);
    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle ><h2>Terms and Conditions</h2></DialogTitle>
            <DialogContent >
                <p>Thank you for submitting this articles, your work is a great asset to the school!</p>
                <br />
                <p>Before this article can be submitted for the Coodinator to see, there are a few Terms and Conditions:</p>
                <div style={{ maxHeight: '200px', overflowY: 'auto' }}>
                    {teacherSubmissionTerms}
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
