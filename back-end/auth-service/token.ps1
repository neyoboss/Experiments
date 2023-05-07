curl --request POST \
  --url 'https://dev-0ck6l5pnflrq01jd.eu.auth0.com/oauth/token' \
  --header 'content-type: application/x-www-form-urlencoded' \
  --data grant_type=client_credentials \
  --data 'client_id=s6lB4cA0A2Cz9pz65tgkpCLt2fjhYgs1' \
  --data 'client_secret={yourClientSecret}' \
  --data 'audience=https://dev-0ck6l5pnflrq01jd.eu.auth0.com/api/v2/'