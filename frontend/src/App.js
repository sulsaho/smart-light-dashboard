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

function App() {
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

  async function getBrightnessValue(e, val) {
    let uri = 'https://localhost:5001/api/LightState/light/brightness/' + val.toString();
    await axios.post(uri);
  }

  async function setUpSunriseSunsetFeature(e, val) {
    let y = await axios.post('https://localhost:5001/api/LightState/light/enable-srss-feature/' + val);
    console.log(y);
  }

  return (
      <div className="App">
        <header className="App-header">
          <div>
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
          </div>

          <div>
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
          </div>

          <h2>Pulse Effect</h2>
          <div>
            <Button
                variant="contained"
                size="large"
                color="primary"
                onClick={breatheEffect}>
              Pulse Light
            </Button>
          </div>

          <h2>Set Brightness</h2>
          <div style={{width: 300}}>
            <Stack spacing={2} direction="row" sx={{ mb: 1 }} alignItems="center">
              <BrightnessLowIcon />
              <Slider
                  color="primary"
                  defaultValue={50}
                  step={10}
                  marks
                  valueLabelDisplay={"auto"}
                  onChange={getBrightnessValue}>
              </Slider>
              <BrightnessHighIcon />
            </Stack>
          </div>

          <h2>Toggle Sunrise and Sunset Feature</h2>
          <div style={{width: 200}}>
            <MaterialUISwitch
                onChange={setUpSunriseSunsetFeature}
                name="srss"
                color="secondary"/>
          </div>

          <div>
            <TextField
                id="sunrise"
                label="Sunrise"
                sx={{ m: 1, width: '25ch' }}
                InputProps={{
                  readOnly: true,
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
                sx={{ m: 1, width: '25ch' }}
                InputProps={{
                  readOnly: true,
                  startAdornment: (
                      <InputAdornment position="start">
                        <NightsStayIcon />
                      </InputAdornment>
                  ),
                }}
            />
          </div>


          {/*<Checkbox/> */}

          {/* Not needed */}
          {/*<img src={logo} className="App-logo" alt="logo" /> */}
        </header>
      </div>
  );
}

export default App;
