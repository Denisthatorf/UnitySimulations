### Instruction


[ML Agent Documentation](https://github.com/Unity-Technologies/ml-agents)

## Using pip 

1. Install [required version of python](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Using-Virtual-Environment.md) (At the time of the last commit, the python version is 3.6 - 3.8)

2.  ```python 
    pip install --upgrade pip && \
    pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu \
    pip install -r requirements.txt
    ```

3. `mlagents-learn config.yaml --results-dir {RESULT_DIR} --run-id={ID_STRING}`

4. If you stop the previous learning process, add `--initialize-from={PREVIOUS_ID_STRING}` to use the previous one as a base for the new one

## Using docker

If you do not want to install another version of python on your computer and you have a docker, you can use it

1. Open ML folder in terminal

2. Build a docker container using the Dockerfile `docker build -t {CONTAINER_NAME} .` 

3. Run container and use current directory as shared folder in docker container `docker run -t -v $(pwd)/data:/result --publish target=5004,published=127.0.0.1:5004,protocol=tcp {CONTAINER_NAME} mlagents-learn config.yaml --results-dir /result --run-id={ID_STRING}` 
    * ML Agent uses sockets to connect to Unity, so we use socket binding `--publish target=5004,published=127.0.0.1:5004,protocol=tcp`.
    * Also we need to make a shared folder so that later we can get our ML model result `-t -v $(pwd)/data:/result`.
    * `--run-id={ID_STRING}` - set the id of ML Agent learning process

4. If you want to use previous module as basic add `--initialize-from={PREVIOUS_ID_STRING}`


[def]: https://github.com/Unity-Technologies/ml-agents