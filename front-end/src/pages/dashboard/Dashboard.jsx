import "./dashboard.css"
import { Bar, Pie } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    registerables 
} from 'chart.js';
import apis from "../../services/apis.service";
import { useEffect, useState } from "react";
import authHeader from "../../services/auth.header";
ChartJS.register(
    ...registerables
);
const Dashboard = () => {
    const [contributorsByYear, setContributorsByYear] = useState(null)
    const [contributionsByYear, setContributionsByYear] = useState(null)
    const [percentageByFaculty, setPercentageByFaculty] = useState(null)
    useEffect(() => {
        const fetchEvent = async () => {
                try {
                    const contributionsResponse = await authHeader().get(apis.faculty + "contributions/by-year" );
                    const contributorsResponse = await authHeader().get(apis.faculty + "contributors/by-year" );
                    const percentageResponse = await authHeader().get(apis.faculty + "contributions/percentage" );
                    setContributionsByYear(contributionsResponse.data)
                    setContributorsByYear(contributorsResponse.data);
                    setPercentageByFaculty(percentageResponse.data)
                } catch (error) {
                    console.error("Error fetching event data:", error);
                }
        };
        fetchEvent();
    }, []);
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
    let percentageData = null;
    if(percentageByFaculty){
        percentageData = {
            labels: percentageByFaculty.map(item => item.faculty),
            datasets: [{
                data: Object.values(percentageByFaculty),
                backgroundColor: Object.keys(percentageByFaculty).map(() => getRandomColor()),
                borderWidth: 1
            }]
        };
    }
    let contributionsResult = null;
    if (contributionsByYear) {
        contributionsResult = {
            labels: contributionsByYear.map(item => item.year),
            datasets: contributionsByYear.reduce((datasets, year) => {
                year.values.forEach(value => {
                    const existingDataset = datasets.find(dataset => dataset.label === value.faculty);
                    if (existingDataset) {
                        existingDataset.data.push(value.value);
                    } else {
                        datasets.push({
                            label: value.faculty,
                            data: [value.value],
                            backgroundColor: getRandomColor(),
                            borderWidth: 1
                        });
                    }
                });
                return datasets;
            }, [])
        };
    }

    let contributorsResult = null;
    if (contributorsByYear) {
        contributorsResult = {
            labels: contributorsByYear.map(item => item.year),
            datasets: contributorsByYear.reduce((datasets, year) => {
                year.values.forEach(value => {
                    const existingDataset = datasets.find(dataset => dataset.label === value.faculty);
                    if (existingDataset) {
                        existingDataset.data.push(value.value);
                    } else {
                        datasets.push({
                            label: value.faculty,
                            data: [value.value],
                            backgroundColor: getRandomColor(),
                            borderWidth: 1
                        });
                    }
                });
                return datasets;
            }, [])
        };
    }

    return (
        <div className="dashboard">
            <h1>Statistics</h1>
            {contributionsByYear && contributorsByYear &&
                (<div className="chartWrapper">
                    <div className="chart">
                        <h3>Number of Contributions by Faculty for Each Academic Year</h3>
                        <Bar
                        data = {contributionsResult}
                        options={options}
                        />
                    </div>
                    <div className="chart">
                        <h3>Number of Contributors by Faculty for Each Academic Year</h3>
                        <Bar
                            data={contributorsResult}
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
            </div>)
        }
        </div>
    );
};

export default Dashboard;
