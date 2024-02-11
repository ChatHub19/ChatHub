<script setup>
import { ref } from 'vue';
import axios from 'axios';

const Modal = {
    props: {
        editMode: Boolean,
        serverToEdit: Object,
    },
    setup(props) {
        const serverName = ref(props.editMode ? props.serverToEdit.name : '');

        const closeServer = () => {
            isModalOpen.value = false;
        };

        const saveServer = () => {
            closeServer();
        };

        return { serverName, closeServer, saveServer };
    },
};
</script>

<template>
    <div class="wrapper" @click="handleDocumentClick">
        <div class="sidebar">
            <font-awesome-icon
                icon="fa-solid fa-plus"
                @click="addServer()"
                @mouseover="showMainIconText = true"
                @mouseleave="showMainIconText = false"
                class="main-icon"
            />
            <div v-show="showMainIconText" class="main-icon-text">Server hinzufügen</div>
            <div v-for="server in servers" :key="server.name" id="server-item">
                <img
                    v-if="server.icon"
                    :src="createObjectURL(server.icon)"
                    class="servers-icon"
                    @mouseover="showServerTooltip(server.name)"
                    @mouseleave="hideServerTooltip"
                    @contextmenu.prevent="showContextMenu(server)"
                />
                <div v-show="server.showTooltip && !server.showContextMenu" class="server-tooltip">
                    {{ server.name }}
                </div>
                <div v-show="server.showContextMenu" class="server-context-menu">
                    <span @click="editServerProfile(server)">Serverprofil bearbeiten</span>
                    <span
                        class="removeServer"
                        @click="removeServer(server)"
                        @mouseover="setHoverEffect(true)"
                        @mouseleave="setHoverEffect(false)"
                        >Server verlassen</span
                    >
                </div>
            </div>
        </div>

        <div v-if="isModalOpen" class="modal" @click="closeServer">
            <div class="modal-content" @click.stop>
                <h2 id="heading">{{ editMode ? 'Server bearbeiten' : 'Server erstellen' }}</h2>
                <h3 id="description">
                    Gib deinem Server mit einem Namen und einem Icon eine ganz eigene
                    Persönlichkeit. Du kannst sie später jederzeit ändern.
                </h3>
                <h4 id="name">SERVERNAME</h4>
                <input
                    ref="serverNameInput"
                    type="text"
                    class="input"
                    v-model="serverName"
                    @input="handleInput"
                    :minlength="1"
                    :maxlength="20"
                    :placeholder="'max. 20 Buchstaben'"
                />
                <input
                    ref="fileInput"
                    type="file"
                    accept=".png, .jpg, .jpeg"
                    style="display: none"
                    @change="handleFileUpload"
                />
                <button @click="uploadServerProfile" id="uploadButton" :disabled="isUploadDisabled">
                    Upload Image
                </button>
                <span v-if="uploadedFileName" id="successUploadProfile">{{
                    uploadedFileName
                }}</span>
                <div class="bottomHalf">
                    <span v-if="duplicateError" id="error-message">Name bereits vorhanden</span>
                    <button
                        ref="saveButton"
                        @click="editMode ? saveEditServer() : saveAddServer()"
                        :disabled="isSaveDisabled"
                        id="saveButton"
                    >
                        {{ editMode ? 'Update' : 'Save' }}
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    methods: {
        setHoverEffect(value) {
            this.isHovering = value;
        },
        createObjectURL(icon) {
            return URL.createObjectURL(icon);
        },
        handleDocumentClick(event) {
            const isOutside =
                !event.target.closest('.sidebar') && !event.target.closest('.server-context-menu');
            if (isOutside) {
                this.hideContextMenu();
            }
        },

        addServer() {
            this.serverName = '';
            this.isModalOpen = true;
            this.selectedFile = null;
            this.uploadedFileName = null;
            this.isUploadDisabled = true;
            this.isSaveDisabled = true;
            this.hideContextMenu();

            this.$nextTick(() => {
                this.$refs.serverNameInput.focus();
            });
        },
        editServerProfile(server) {
            this.isModalOpen = true;
            this.editMode = true;
            this.serverToEdit = server;
            this.serverName = server.name;
            this.hideContextMenu();

            this.$nextTick(() => {
                this.$refs.serverNameInput.focus();
            });
        },
        removeServer(server) {
            const index = this.servers.indexOf(server);
            if (index !== -1) {
                this.servers.splice(index, 1);
            }
            this.hideContextMenu();
        },
        closeServer() {
            this.selectedFile = null;
            this.duplicateError = false;
            this.isModalOpen = false;
        },

        async saveAddServer() {
            if (this.serverName && this.serverName.trim() !== '') {
                const newServer = {
                    name: this.serverName.trim(),
                    icon: this.selectedFile,
                    // guid: this.$store.state.user.guid,
                };
                this.servers.push(newServer);
                // try {
                //     console.log(newServer);
                //     const serverdata = await axios.post('server/add_server', newServer);
                // } catch (e) {}
            }
            this.editMode = false;
            this.isModalOpen = false;
        },
        saveEditServer() {
            if (this.serverName && this.serverName.trim() !== '') {
                const editedServer = {
                    name: this.serverName.trim(),
                    icon: this.selectedFile,
                };
                const index = this.servers.findIndex((s) => s === this.serverToEdit);
                if (index !== -1) {
                    this.servers[index].name = editedServer.name;
                    this.servers[index].icon = editedServer.icon;
                }
            }
            this.isModalOpen = false;
            this.editMode = false;
        },

        showServerTooltip(serverName) {
            const server = this.servers.find((s) => s.name === serverName);
            if (server) {
                server.showTooltip = true;
            }
        },
        hideServerTooltip() {
            this.servers.forEach((server) => {
                server.showTooltip = false;
            });
        },

        showContextMenu(server) {
            this.hideContextMenu();
            server.showTooltip = false;
            server.showContextMenu = true;
        },
        hideContextMenu() {
            this.servers.forEach((server) => {
                server.showContextMenu = false;
            });
        },

        handleInput(event) {
            let inputValue = event.target.value;
            const isDuplicate = this.servers.some(
                (server) =>
                    server !== this.serverToEdit &&
                    server.name.trim().toLowerCase() === inputValue.trim().toLowerCase()
            );
            this.isUploadDisabled = inputValue === '' || isDuplicate;
            this.duplicateError = isDuplicate;
            this.isSaveDisabled = this.isUploadDisabled || !this.selectedFile;

            if (inputValue.length > 10) {
                inputValue = inputValue.replace(/(.{10})/g, '$1\n');
            }
            this.serverName = inputValue;
        },
        handleFileUpload(event) {
            const fileInput = event.target;
            this.selectedFile = fileInput.files[0];
            this.isSaveDisabled = this.selectedFile != null ? false : true;
            this.uploadedFileName = this.selectedFile ? this.selectedFile.name : null;
            this.$nextTick(() => {
                this.$refs.saveButton.focus();
            });
        },
        uploadServerProfile() {
            this.$refs.fileInput.click();
        },
    },
    data() {
        return {
            servers: [],
            editMode: false,
            serverToEdit: null,
            isModalOpen: false,
            duplicateError: false,
            uploadedFileName: null,
            isUploadDisabled: true,
            isSaveDisabled: true,
            isHovering: false,
            showMainIconText: false,
        };
    },
};
</script>


<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Nunito+Sans:opsz,wght@6..12,200;6..12,400&display=swap');

.sidebar {
    min-height: 100vh;
    width: 65px;
    text-align: center;
    padding-top: 1rem;
    background: #201c24;
}

.main-icon,
.servers-icon {
    position: relative;
    width: 1.55rem;
    height: 1.75rem;
    font-size: 1.75rem;
    border-radius: 50%;
    padding: 0.75rem;
    background: #38343c;
    color: #7b84f2;
    transition: border-radius 0.35s;
}

.main-icon-text {
    position: absolute;
    background: #181414;
    color: #fff;
    padding: 0.5rem;
    border-radius: 5px;
    font-size: 16px;
    top: 1.325rem;
    transform: translateX(55.5%);
    white-space: normal;
}

.servers-icon {
    width: 48.8px;
    height: 52px;
    padding: 0 !important;
    background: none;
}

.main-icon:hover {
    border-radius: 32.5%;
    cursor: pointer;
    background: #7464f1;
    color: #fff;
}

.servers-icon:hover {
    border-radius: 32.5%;
    cursor: pointer;
}

#server-item {
    margin-top: 10px;
    position: relative;
}

#server-item:hover::before {
    content: '';
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    left: -5px;
    height: 40%;
    width: 10px;
    background-color: #fff;
    border-radius: 32.5%;
}

.server-tooltip {
    position: absolute;
    background: #181414;
    color: #fff;
    padding: 0.5rem;
    border-radius: 5px;
    font-size: 16px;
    right: -15px;
    top: 50%;
    transform: translateY(-50%) translateX(100%);
    white-space: normal;
}

.server-context-menu {
    text-align: left;
    position: absolute;
    background: #181414;
    color: #fff;
    padding: 0.5rem;
    border-radius: 5px;
    font-size: 16px;
    right: 25px;
    top: 150%;
    transform: translateY(-50%) translateX(100%);
    white-space: nowrap;
    cursor: pointer;
    z-index: 1000;
}

.server-context-menu span {
    display: block;
    padding: 0.5rem;
}

.server-context-menu span:hover {
    background: #737de5;
    color: #fff;
    border-radius: 5px;
}

.removeServer {
    color: #ff0000;
}
.removeServer:hover {
    background: #ff0000 !important;
    color: #fff;
    border-radius: 5px;
}

.modal {
    position: fixed;
    top: 0;
    left: 0;
    height: 100%;
    width: 100%;
    background: #00000080;
    display: flex;
    justify-content: center;
    align-items: center;
    overflow: hidden;
}

.modal-content {
    position: absolute;
    width: 35rem;
    font-family: 'Nunito Sans';
    color: #000;
    background: #fff;
    padding: 20px;
    border-radius: 10px;
}

#heading {
    font-size: 30px;
    font-weight: bold;
    text-align: center;
}

#description {
    color: #8c8c8c;
    line-height: 20px;
    padding-bottom: 1rem;
}

#name {
    color: #50535a;
    font-weight: bold;
}

.input {
    background: #f0ecec;
    border-style: none;
    height: 2.5rem;
    width: 100%;
    margin-bottom: 1.5rem;
}

.input:focus {
    outline: none;
}

.input[type='text'] {
    font-size: 18px;
    color: #50535a;
    padding: 10px;
}

#uploadButton {
    margin: 0rem 0rem 3.5rem 0rem;
    font-size: 17px;
    font-family: 'Nunito Sans';
}

#successUploadProfile {
    padding-left: 0.5rem;
    opacity: 0.5;
    font-size: 17px;
    font-family: 'Nunito Sans';
}

.bottomHalf {
    background: #f0ecec;
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
    padding: 1rem;
    margin: 0px -20px -20px -20px;
    display: flex;
    justify-content: flex-end;
    align-items: center;
}

#error-message {
    color: #ff0000;
    margin-right: auto;
    margin-left: 0.4em;
}

#saveButton {
    font-size: 17px;
    font-family: 'Nunito Sans';
}
</style>    