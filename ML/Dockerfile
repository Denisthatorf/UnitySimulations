FROM python:3.6

COPY . .

EXPOSE 5004

RUN mkdir result

RUN pip install --upgrade pip && \
    pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu \
    pip install -r requirements.txt
