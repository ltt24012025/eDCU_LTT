# eDCU LTT

## Release Notes

### LTT Version 1.1.0.24 — Release

#### Modifications

1. Raw Power Monitoring was mapped with different digital inputs and relay outputs of the Power Monitoring Module provided by Marywin.
2. All PDU Power Monitoring signals were mapped with the power supply IP ping status.

---

### LTT Version 1.1.0.23 — Release

#### New Features

1. **`RMS Current Check.vi`**
   - Removed the error case to ensure that input errors do not stop RMS reading.

2. **`Wait and Monitor_UPS.vi`**
   - Updated the user interface.
   - Added elapsed-time data to the UI.
   - Added an `RMS_refresh` indicator to the UI.

#### Fixes

1. Fixed missing error data for `FE`/`FF` frames in case of decoding issues.
   - **Verification status:** Needs verification.

2. Made the monitor duration configurable in the **Wait Monitor** step using the station global.
   - **Verification status:** Fixed; needs verification.

3. Improved RMS reading frequency in the **Wait Monitor** step by changing the `For Loop` to a parallel `For Loop`.
   - **Verification status:** Fixed; needs verification.

---

### Known Issues in Version 1.1.0.22

1. No known issues documented.

---

### LTT Version 1.1.0.21 — Release

#### Fixes

1. Added a default user ID for VT login, as specified in the User Manual.
   - **Verification status:** Fixed.

2. Fixed the issue preventing sockets from running independently.
   - **Verification status:** Fixed and verified.

3. Fixed the issue where CAN logs were not visible in Manual Mode.
   - **Verification status:** Fixed and verified.

4. Renamed Mission Mode in Manual Mode to make the naming more generic.
   - **Verification status:** Fixed and verified.

5. Fixed the **Wait Monitor** step issue where Raw Power Monitoring was hard-coded to be ignored only when `MODE = 0`.
   - **Verification status:** Fixed and verified.

#### New Features

- None.

---

### Known Issues in Version 1.1.0.21

1. No known issues documented.
