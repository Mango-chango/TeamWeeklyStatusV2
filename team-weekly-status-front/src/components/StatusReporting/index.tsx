import React, { useEffect, useState } from "react";
import { userStore } from "../../store";
import { makeApiRequest } from "../../services/apiHelper";
import { WeeklyStatusData, TeamWeeklyStatusData } from "../../types/WeeklyStatus.types";
import moment from "moment";

const StatusReporting: React.FC = () => {
    const { role, teamName, memberName } = userStore();
    const [teamWeeklyStatusData, setTeamWeeklyStatusData] =
    useState<TeamWeeklyStatusData | null>(null);

    const initialStartDate = moment().startOf("week").toDate();
    const [startDate, setStartDate] = useState(initialStartDate);
  
    const endDate = moment().endOf("week").toDate();
  
    useEffect(() => {
        const fetchExistingStatus = async () => {
          const requestData = {
            memberId: null,
            weekStartDate: startDate.toISOString(),
          };
          const response: TeamWeeklyStatusData = await makeApiRequest(
            "/WeeklyStatus/GetAllWeeklyStatusesByStartDate",
            "POST",
            requestData
          );
    
          if (response) {
            setTeamWeeklyStatusData(response);
          }
        };
    }, [startDate]);

    return <></>    
}

export default StatusReporting;