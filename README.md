#Introduction 
It's a tiny tool to help teachers create semi-random groups. No UI, run system test to generate output as txt.

## Running the App

### 1. Start the API

```bash
dotnet run --project src/AutoTeam.Api
```

The API will start on **http://localhost:5000**.

### 2. Open the Frontend

Open `src/AutoTeam.Web/index.html` in your browser (just double-click the file or use a local file server).

The frontend communicates with the API at `http://localhost:5000`. If the API is running on a different port, update the `API_BASE` constant at the top of the `<script>` block in `index.html`.