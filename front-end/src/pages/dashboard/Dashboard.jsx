import React from 'react';
import { Bar, Pie } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    registerables 
} from 'chart.js';
ChartJS.register(
    ...registerables
);
const Dashboard = () => {
    const contributionsByYear = {
        "2022": {
            "Faculty A": 50,
            "Faculty B": 30,
            "Faculty C": 20
        },
        "2023": {
            "Faculty A": 60,
            "Faculty B": 40,
            "Faculty C": 25
        },
        "2024": {
            "Faculty A": 70,
            "Faculty B": 45,
            "Faculty C": 30
        }
        };
        
        const percentageByFaculty = {
        "Faculty A": 40,
        "Faculty B": 30,
        "Faculty C": 20,
        "Faculty D": 10
        };
        
        const contributorsByYear = {
        "2022": {
            "Faculty A": 25,
            "Faculty B": 15,
            "Faculty C": 10
        },
        "2023": {
            "Faculty A": 30,
            "Faculty B": 20,
            "Faculty C": 15
        },
        "2024": {
            "Faculty A": 35,
            "Faculty B": 25,
            "Faculty C": 20
        }
        };
    const options = {
        plugins: {
            title: {
                display: true,
                text: 'Bar Chart'
            }
        },
    };
    const getRandomColor = () => {
        const letters = '0123456789ABCDEF';
        let color = '#';
        for (let i = 0; i < 6; i++) {
          color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    };
    const percentageData = {
        labels: Object.keys(percentageByFaculty),
        datasets: [{
            data: Object.values(percentageByFaculty),
            backgroundColor: Object.keys(percentageByFaculty).map(() => getRandomColor()),
            borderWidth: 1
        }]
    };
    const contributionsData = {
    labels: Object.keys(contributionsByYear),
    datasets: Object.keys(contributionsByYear[Object.keys(contributionsByYear)[0]]).map(faculty => ({
        label: faculty,
        data: Object.values(contributionsByYear).map(obj => obj[faculty]),
        backgroundColor: getRandomColor(),
        borderWidth: 1
    }))
    };
    console.log(contributionsData)
    // Convert data for contributors by year into datasets for chart
    const contributorsData = {
    labels: Object.keys(contributorsByYear),
    datasets: Object.keys(contributorsByYear[Object.keys(contributorsByYear)[0]]).map(faculty => ({
        label: faculty,
        data: Object.values(contributorsByYear).map(obj => obj[faculty]),
        backgroundColor: getRandomColor(),
        borderWidth: 1
    }))
    };

    return (
        <div className="dashboard">
            <h2>Statistics</h2>
            <div className="chart">
                <h3>Number of Contributions by Faculty for Each Academic Year</h3>
                <Bar
                data={contributionsData}
                options={options}
                />
            </div>
            <div className="chart">
                <h3>Number of Contributors by Faculty for Each Academic Year</h3>
                <Bar
                    data={contributorsData}
                    options={options}
                />
            </div>
            <div className="chart">
                <h3>Percentage of Contributions by Each Faculty</h3>
                <Pie
                    data={percentageData}
                    options={options}
                />
            </div>
        </div>
    );
};

export default Dashboard;
