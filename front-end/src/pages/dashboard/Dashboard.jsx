import "./dashboard.css"
import { Bar, Pie } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    registerables 
} from 'chart.js';
import useFetch from "../../hooks/useFetch";
import apis from "../../services/apis.service";
import { useEffect, useState } from "react";
import authHeader from "../../services/auth.header";
ChartJS.register(
    ...registerables
);
const Dashboard = () => {
    // const contributionsByYear = {
    //     "2022": {
    //         "Faculty A": 50,
    //         "Faculty B": 30,
    //         "Faculty C": 20
    //     },
    //     "2023": {
    //         "Faculty A": 60,
    //         "Faculty B": 40,
    //         "Faculty C": 25
    //     },
    //     "2024": {
    //         "Faculty A": 70,
    //         "Faculty B": 45,
    //         "Faculty C": 30
    //     }
    // };
    // const {contributions, contributionsLoading, contributionsError, contributionsRefetch} = useFetch(apis.faculty+"contributions/by-year")
    const [contributorsByYear, setContributorsByYear] = useState(null)
    const [contributionsByYear, setContributionsByYear] = useState(null)
    useEffect(() => {
        const fetchEvent = async () => {
                try {
                    const contributionsResponse = await authHeader().get(apis.faculty + "contributions/by-year" );
                    const contributorsResponse = await authHeader().get(apis.faculty + "contributors/by-year" );
                    setContributionsByYear(contributionsResponse.data)
                    setContributorsByYear(contributorsResponse.data);
                } catch (error) {
                    console.error("Error fetching event data:", error);
                }
        };
        fetchEvent();
    }, []);
    console.log(contributorsByYear)
        const percentageByFaculty = {
        "Faculty A": 40,
        "Faculty B": 30,
        "Faculty C": 20,
        "Faculty D": 10
        };
        
        // const contributorsByYear = {
        // "2022": {
        //     "Faculty A": 25,
        //     "Faculty B": 15,
        //     "Faculty C": 10
        // },
        // "2023": {
        //     "Faculty A": 30,
        //     "Faculty B": 20,
        //     "Faculty C": 15
        // },
        // "2024": {
        //     "Faculty A": 35,
        //     "Faculty B": 25,
        //     "Faculty C": 20
        // }
        // };
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
    console.log(contributionsResult)

    // Convert data for contributors by year into datasets for chart
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
