# Tomplay Technical Challenge: The Playlist Builder

**Role:** Senior Unity Engineer (UI Focus)
**Estimated Time:** 2-3 Hours

## The Mission

You are prototyping a **"Playlist Builder"** feature. The user needs to browse the catalogue of available scores and organize them into one of their cloud playlists.

### The Requirements

#### 1. The Service (Provided in Project)

We have provided a script `ScoreService.cs` (Namespace: `Cloud`) in the template project. This script simulates a backend API with database access. It is accessible via the singleton `ScoreService.Instance`.

- **Reference:** We have provided a working example scene at `Assets/Scenes/ScoreServiceUsageExample.unity` (and `ScoreServiceUsageExample.cs`). Please run this scene to understand how to call the API and read the data.
- **Methods Available:**
    - `FetchScoresFromCatalogue(string filter, CancellationToken)`: Fetches scores from the global database. The server limits results to 50 items.
    - `GetUserPlaylistIds(CancellationToken)`: Returns the IDs of playlists owned by the user.
    - `GetUserPlaylist(int playlistId, CancellationToken)`: Returns the specific scores currently inside a playlist.
    - `UpdatePlaylist(int playlistId, List<int> scoreIds, CancellationToken)`: Syncs the modified playlist to the server.
- **Constraint:** You should treat this class as if it were in a separate assembly (Black Box). You may not change its logic.
- **Behavior:** It is slow (latency) and unreliable (throws exceptions ~20% of the time).

#### 2. The User Interface

Using **Unity UI Toolkit (UXML/USS)**, create a responsive screen split into two main areas.

**Reference:** We have provided a mockup file in the root folder: `interface_design_mockup.html`. Please use this as a guide for the layout.
> **Note:** We are primarily judging your **architecture and code quality**, so visuals will be judged less harshly than the logic. However, we do value a good eye for design, so please attempt to make the interface look clean and polished.

- **Library (Left):**
    - **Search Bar:** A text input to filter the catalogue.
    - **List:** Displays available scores fetched from the server. Each item must show: Title, Composer, Difficulty.
- **My Playlist (Right):** Displays the scores inside the currently selected playlist.
    - Must show a "Total Duration" summary at the bottom formatted as `MM:SS`.

**Responsiveness:**
The UI must handle scaling gracefully. It is strictly required that the layout remains usable and readable in both **16:9 (Landscape)** and **1:1 (Square)** aspect ratios without elements overlapping.

#### 3. Functionality & Logic

- **Initialization (Async Chaining):**
    1.  On start, fetch the user's playlist IDs using `GetUserPlaylistIds`.
    2.  Load the **first playlist** returned by that list.
    3.  Display the scores from that playlist in the Right Panel.
    4.  *Simultaneously*, load the initial default list of scores for the Library (Left Panel).
    5.  You must handle **Loading States** and **Server Failures** visually (e.g., "Retry" button) for all these requests.
- **Search & Filtering (Crucial):**
    - Because the server limits the number of results, you must implement the Search function calling `FetchScoresFromCatalogue(filterString)`.
    - The search should trigger when the text changes (or is submitted).
    - **Do not implement local paging.** Rely on the filter to find specific items.
- **Interaction:** Clicking a "+" button on an item in the Library adds it to the active Playlist (Right Panel).
- **Pro Label:** Any score longer than **4 minutes** is considered a "Pro" score. These items must have a distinct visual marker (e.g., a gold border or "PRO" badge) in the list.
- **Reactivity:** The "Total Duration" must update immediately when items are added or removed.
- **Saving:** When the playlist changes, you must call `UpdatePlaylist` to sync the changes to the active Playlist ID.

### Technical Constraints (Strict)
To ensure this code is production-ready, you must adhere to these constraints:

#### 1. No UI Logic in Data Classes
Your core logic classes (Data Managers, Service wrappers) must **not** reference the `UnityEngine.UIElements` namespace. They should handle data only.

#### 2. Controller/View Separation
You may update the UI manually (querying elements) or use binding, but your UI code should be distinct from your business logic.

#### 3. Non-Blocking & Cancellation
The UI must never freeze. Additionally, since the Service accepts a `CancellationToken`, you should use it to cancel stale requests (e.g., fast typing in the search bar).

#### 4. Strict UI Toolkit
Do not use IMGUI (`OnGUI`) or the old Canvas system (`UGUI`).

### Bonus Points (Nice to have)
If you have extra time, we love seeing attention to detail:

1.  **Juicing & Polish:** Add visual feedback (animations, tweens, or particle effects) when items are added to the playlist. Make the app "feel" good to use.
2.  **Drag & Drop:** Allow the user to reorder items inside the Playlist.
3.  **Advanced Responsiveness:** While 16:9 and 1:1 are required, bonus points if you implement a full layout shift (e.g., stacking panels vertically) for **Portrait (9:16)**.

### Deliverables

#### 1. The Unity Project
A zip file or git repo.

#### 2. The Walkthrough (Mandatory)
A short (2-3 minute) video recording (Google Drive, YouTube unlisted, etc.) where you:

- Show the app running.
- **Explain your architecture:** Walk us through your scripts and explain *how* the data moves from the Service to the UI. Explain *why* you organized the code this way.

*(Note: We value your ability to explain your technical decisions as much as the code itself.)*