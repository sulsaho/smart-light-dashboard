import './App.css';
import Button from '@material-ui/core/Button';
import ButtonGroup from '@material-ui/core/ButtonGroup'
import Slider from '@material-ui/core/Slider';
import BrightnessLowIcon from '@mui/icons-material/BrightnessLow';
import BrightnessHighIcon from '@mui/icons-material/BrightnessHigh';
import axios from 'axios';
import {Stack} from "@mui/material";
import LightModeIcon from '@mui/icons-material/LightMode';
import NightsStayIcon from '@mui/icons-material/NightsStay';
import MaterialUISwitch from './MUISwitch'
import {InputAdornment, TextField} from "@material-ui/core";
import React, { useState, useEffect} from "react";
import {VictoryChart, VictoryLabel, VictoryLine, VictoryTheme} from "victory";

function App() {

  const [isSrssEnabled, setIsSrssEnabled]= useState(false);
  const [stats, setStats] = useState([]);
  const [utility, setUtility] = useState([]);
  const [brightnessList, setBrightnessList] = useState([]);
  const [defBrightness, setDefaultBrightness] = useState(0);
  const [firstTimeStamp, setFirstTimeStamp] = useState([]);
  const [currentState, setCurrentState] = useState('');
  const [runningTime, setRunningTime] = useState('');
  const [usageAmount, setUsageAmount] = useState('');


  useEffect(() => {
    const handleLoad = async (event) => {
      let result = await axios.post('https://localhost:5001/api/LightState/light/fetch-srss-feature/');
      let enabled = JSON.stringify(result.data) !== '{}' ? result.data.srss_feature.enabled : false;

      setIsSrssEnabled( enabled );
      setSrssComponents(enabled, result.data);
    };
    window.addEventListener('load', handleLoad);
    return () => {
      window.removeEventListener('load', handleLoad);
    };
  }, []);

  useEffect( () => {
    async function fetchData (){
      const response = await axios.post('https://localhost:5001/api/LightState/light/stats');
      setStats(response.data);
    }
    fetchData();
  }, [])

  useEffect( () => {
    async function fetchData (){
      const response = await axios.post('https://localhost:5001/api/LightState/light/utility');
      setUtility(response.data);
    }
    fetchData();
  }, [])

  useEffect( () => {
    async function fetchData (){
      const response = await axios.post('https://localhost:5001/api/LightState/light/brightness-list');
      setBrightnessList(response.data);
    }
    fetchData();
  }, [])

  useEffect( () => {
    async function fetchData (){
      const response = await axios.post('https://localhost:5001/api/LightState/light/initial-timestamp');
      setFirstTimeStamp(response.data);
    }
    fetchData();
  }, [])

  useEffect( () => {
    async function fetchData (){
      const response = await axios.post('https://localhost:5001/api/LightState/light/current-brightness');
      setDefaultBrightness(response.data);
    }
    fetchData();
  }, [])

  useEffect( () => {
    async function fetchData (){
      const response = await axios.post('https://localhost:5001/api/LightState/light/current-state');
      setCurrentState(response.data);
    }
    fetchData();
  }, [])



  async function onButton() {
    await axios.post('https://localhost:5001/api/LightState/light/turn-on');
  }

  async function offButton() {
    await axios.post('https://localhost:5001/api/LightState/light/turn-off');
  }

  async function setWhiteColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/white');
  }

  async function setRedColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/red');
  }

  async function setOrangeColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/orange');
  }

  async function setYellowColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/yellow');
  }

  async function setCyanColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/cyan');
  }

  async function setGreenColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/green');
  }

  async function setBlueColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/blue');
  }

  async function setPurpleColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/purple');
  }

  async function setPinkColor() {
    await axios.post('https://localhost:5001/api/LightState/light/color/pink');
  }

  async function breatheEffect() {
    await axios.post('https://localhost:5001/api/LightState/light/breathe');
  }

  async function getBrightnessValue(e, val){
    await axios.post(`https://localhost:5001/api/LightState/light/brightness/${val.toString()}`);
  }

  async function setUpSunriseSunsetFeature(e, val) {
    setIsSrssEnabled( val );
    let result = await axios.post('https://localhost:5001/api/LightState/light/enable-srss-feature/' + val);
    setSrssComponents(val, result.data);
  }

  function setSrssComponents(toFill, data) {
    let sunriseElement = document.getElementById("sunrise");
    let sunsetElement = document.getElementById("sunset");
    if (toFill) {
      sunriseElement.value = data.srss_feature.sunrise;
      sunsetElement.value = data.srss_feature.sunset;
    }
    else {
      sunriseElement.value = '';
      sunsetElement.value = '';
    }
  }

  async function setUtilTime() {
    if (currentState === 'on')
    {
      setRunningTime(utility[2]);
      setUsageAmount(utility[3]);
    }
    else
    {
      setRunningTime(utility[0]);
      setUsageAmount(utility[1]);
    }
  }

  const interval = setInterval(function() {
    setUtilTime();
  }, 30000);

  clearInterval();

  return (
      <div className="App">
        <header className="App-header">
          <h1>Controls</h1>
          <div className="Controls">

            <div className="Inner-controls">
              <Stack spacing={2} direction="column" sx={{ mb: 1 }} alignItems="center">
                <h2>Turn ON/OFF</h2>
                <ButtonGroup
                    variant="outlined">
                  <Button
                      size="large"
                      color="primary"
                      onClick={onButton}>
                    ON
                  </Button>
                  <Button
                      size="large"
                      color="secondary"
                      onClick={offButton}>
                    OFF
                  </Button>
                </ButtonGroup>
              </Stack>
            </div>


            <div className="Inner-controls">
              <Stack spacing={2} direction="column" sx={{ mb: 1 }} alignItems="center">
                <h2>Set Colors</h2>
                <ButtonGroup
                    variant='outlined'>
                  <Button
                      size="large"
                      style={{color : '#FFFFFF'}}
                      onClick={setWhiteColor}>
                    White
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#FF0000'}}
                      onClick={setRedColor}>
                    Red
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#FFA500'}}
                      onClick={setOrangeColor}>
                    Orange
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#FFFF00'}}
                      onClick={setYellowColor}>
                    Yellow
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#00FFFF'}}
                      onClick={setCyanColor}>
                    Cyan
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#008000'}}
                      onClick={setGreenColor}>
                    Green
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#0000FF'}}
                      onClick={setBlueColor}>
                    Blue
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#800080'}}
                      onClick={setPurpleColor}>
                    Purple
                  </Button>
                  <Button
                      size="large"
                      style={{color : '#FF00FF'}}
                      onClick={setPinkColor}>
                    Pink
                  </Button>
                </ButtonGroup>
              </Stack>
            </div>


            <div className="Inner-controls">
              <Stack spacing={2} direction="column" sx={{ mb: 1 }} alignItems="center">
                <h2>Breathe Effect</h2>
                <Button
                    variant="outlined"
                    size="large"
                    color="primary"
                    onClick={breatheEffect}>
                  Breathe Light Effect
                </Button>
              </Stack>
            </div>

            <div className="Inner-controls">
              <Stack spacing={2} direction="column" sx={{ mb: 1 }} alignItems="center">
                <h2>Set Brightness</h2>
                <div style={{width: 300}}>
                  <Stack spacing={2} direction="row" sx={{ mb: 1 }} alignItems="center">
                    <BrightnessLowIcon />
                    <Slider
                        color="primary"
                        defaultValue={defBrightness}
                        step={10}
                        marks
                        valueLabelDisplay={"auto"}
                        onChange={getBrightnessValue}>
                    </Slider>
                    <BrightnessHighIcon />
                  </Stack>
                </div>
              </Stack>
            </div>
          </div>
          <hr></hr>

          <h1>Features</h1>
          <div className="Features">
            <Stack spacing={2} direction="column" sx={{ mb: 1 }} alignItems="center">
              <h2> Sunrise/Sunset </h2>
              <div>
                <div style={{width: 200}}>

                  <MaterialUISwitch
                      checked={isSrssEnabled}
                      onChange={setUpSunriseSunsetFeature}
                      id="srss"
                      color="secondary"/>
                </div>

                <div style={{width: 460, margin: "1% 0 10% 5%", display: "flex", justifyContent: "space-around"}}>
                  <TextField
                      id="sunrise"
                      label="Sunrise"
                      variant="filled"
                      focused
                      InputProps={{
                        readOnly: true,
                        style: {
                          color: 'white'
                        },
                        startAdornment: (
                            <InputAdornment position="start">
                              <LightModeIcon />
                            </InputAdornment>
                        ),
                      }}
                  />

                  <TextField
                      id="sunset"
                      label="Sunset"
                      variant="filled"
                      focused
                      InputProps={{
                        readOnly: true,
                        style: {
                          color: 'brown'
                        },
                        startAdornment: (
                            <InputAdornment position="start">
                              <NightsStayIcon />
                            </InputAdornment>
                        ),
                      }}
                  />
                </div>
              </div>
            </Stack>
          </div>
          <hr></hr>

          <h1>Stats</h1>
          <div className="Stats">
            <div className="Inner-stats">
              <Stack spacing={2} direction="column" sx={{ mb: 1 }} alignItems="center">
                <h2>Advanced Stats:</h2>
                <ul>
                  {stats.map(stat => <li key={stat}>{stat}</li>)}
                </ul>
              </Stack>
            </div>
            <div className="Inner-stats">
              <h2>Utility:</h2>
              <p>As of {firstTimeStamp},<br></br>
                based on standby wattage of ~0.7W,<br></br>
                and average wattage of ~7W,<br></br>
                with running time of {runningTime} hours<br></br>
                will cost ${usageAmount} based on<br></br>
                Cass County Electric's figures
              </p>
            </div>
            <div className="Inner-stats">
              <h2>Brightness Levels:</h2>
              <div>
                <VictoryChart theme={VictoryTheme.material} domainPadding={20} width={1000}>
                  <VictoryLabel text={`Brightness levels as of ${firstTimeStamp}`} x={70} y={30}/>
                  <VictoryLine interpolation="natural" data={brightnessList} x={'category'} y={'count_mins'}/>
                </VictoryChart>
              </div>
            </div>
          </div>


          {/* Not needed */}
          {/*<img src={logo} className="App-logo" alt="logo" /> */}
        </header>
      </div>
  );
}

export default App;
