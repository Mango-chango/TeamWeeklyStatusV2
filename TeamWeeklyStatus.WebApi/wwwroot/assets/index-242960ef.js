import{u,r as s,j as n,G as d,A as m,m as p,a as o}from"./index-2375c4d2.js";const h=()=>{const i=u();s.useState(""),s.useState("@mangochango.com");const[r,a]=s.useState(null),g=()=>{const t=document.createElement("script");t.src="https://accounts.google.com/gsi/client",t.onload=()=>{window.google.accounts.id.initialize({client_id:"91039693581-hprbpbenb5fjgm5ccq73d72cpu1o4ptf.apps.googleusercontent.com",callback:c}),window.google.accounts.id.renderButton(document.getElementById("buttonDiv"),{theme:"outline",size:"large"})},document.body.appendChild(t)};s.useEffect(()=>{g()},[]);const c=async t=>{const l=t.credential;console.log("idToken=",l);try{const e=await p("/GoogleAuth","POST",{idToken:l});e&&e.success?(o.getState().setRole(e.role),o.getState().setTeamName(e.teamName),o.getState().setMemberId(e.memberId),o.getState().setMemberName(e.memberName),o.getState().setIsAuthenticated(!0),i("/weekly-status")):a("Could not authenticate with Google. Please try again.")}catch(e){console.error("Google login error:",e),a("An unexpected error occurred. Please try again.")}};return n.jsxs("div",{className:"d-flex flex-column align-items-center mt-5",children:[n.jsx(d,{"data-testid":"google-login",onSuccess:c,onError:()=>a("Google Sign-In was unsuccessful. Try again later.")}),r&&n.jsx(m,{variant:"danger",className:"mt-3 w-300",children:r})]})};export{h as default};
